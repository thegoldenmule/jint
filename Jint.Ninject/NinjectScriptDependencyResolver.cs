using System;
using System.Collections.Generic;
using Jint.Unity;
using Ninject;

namespace Jint.Ninject
{
    /// <summary>
    /// Provides script dependencies through Ninject.
    /// </summary>
    public class NinjectScriptDependencyResolver : IScriptDependencyResolver
    {
        /// <summary>
        /// Kernel.
        /// </summary>
        private readonly IKernel _kernel;

        /// <summary>
        /// Dependencies by name.
        /// </summary>
        private readonly Dictionary<string, Type> _namedDependencies = new Dictionary<string, Type>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="kernel"></param>
        public NinjectScriptDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;

            FindNamedDependencies();
        }

        /// <summary>
        /// Resolves a dependency by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Resolve(string name)
        {
            Type type;
            if (_namedDependencies.TryGetValue(name, out type))
            {
                return _kernel.Get(type);
            }

            // TODO: Try to find by name.

            return null;
        }

        /// <summary>
        /// Crawls types and pulls out any with a NinjectScriptDependencyAttribute.
        /// </summary>
        private void FindNamedDependencies()
        {
            // search all assemblies for named dependencies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0, ilen = assemblies.Length; i < ilen; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0, jlen = types.Length; j < jlen; j++)
                {
                    var type = types[j];
                    var attributes = type.GetCustomAttributes(
                        typeof(NinjectScriptDependencyAttribute),
                        true);
                    if (attributes.Length > 0)
                    {
                        var attribute = attributes[0];
                        _namedDependencies[((NinjectScriptDependencyAttribute)attribute).Name] = type;
                    }
                }
            }
        }
    }
}