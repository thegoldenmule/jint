using System;
using Jint;
using Jint.Native;
using Jint.Runtime;
using UnityEngine;

namespace JintUnity
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
        /// Sequential ids for requires.
        /// </summary>
        private int _ids = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnityScriptingHost()
            : base(options => options.AllowClr())
        {
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
            var resource = Resources.Load<TextAsset>(scriptName);
            if (null == resource)
            {
                return JsValue.Undefined;
            }

            // modularize it
            var variableName = "require" + _ids++;
            var moduleCode = REQUIRE_TEMPLATE
                .Replace("{{script}}", resource.text)
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

        private object Inject(string name)
        {
            return null;
        }
    }
}