using System;
using Jint.Native;
using UnityEngine;

namespace Jint.Unity
{
    /// <summary>
    /// Hosts scripts and provides a default Unity API.
    /// </summary>
    public class UnityScriptingHost : Engine
    {
        /// <summary>
        /// Template for executing require'd scripts.
        /// </summary>
        private const string REQUIRE_TEMPLATE = @"
// prep modules
var module = {
    exports : {
        //
    }
};

// execute require
(function() {
    {{script}}
})();

var {{variableName}} = module.exports;
module = null;
";

        /// <summary>
        /// Loads scripts.
        /// </summary>
        private readonly IScriptLoader _loader;

        /// <summary>
        /// Object to resolve dependencies.
        /// </summary>
        private readonly IScriptDependencyResolver _resolver;

        /// <summary>
        /// Sequential ids for requires.
        /// </summary>
        private int _ids = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnityScriptingHost(
            IScriptLoader loader,
            IScriptDependencyResolver resolver)
            : base(options => options.AllowClr())
        {
            _loader = loader;
            _resolver = resolver;

            SetValue("Log", new UnityLogWrapper());
            SetValue("Scene", new UnitySceneManager());
            
            SetValue("require", new Func<string, JsValue>(Require));
            SetValue("inject", new Func<string, object>(Inject));
        }

        /// <summary>
        /// Loads script through resources + executes.
        /// </summary>
        /// <param name="scriptName"></param>
        private JsValue Require(string scriptName)
        {
            string script;
            if (!_loader.Load(scriptName, out script))
            {
                return JsValue.Undefined;
            }
            
            // modularize it
            var variableName = "require" + _ids++;
            var moduleCode = REQUIRE_TEMPLATE
                .Replace("{{script}}", script)
                .Replace("{{variableName}}", variableName);

            JsValue module;
            try
            {
                Execute(moduleCode);
                module = GetValue(variableName);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);

                return JsValue.Undefined;
            }

            return module;
        }

        /// <summary>
        /// Injector.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private object Inject(string name)
        {
            return null;
        }
    }
}