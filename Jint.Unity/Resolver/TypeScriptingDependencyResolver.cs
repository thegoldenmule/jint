using System;

namespace Jint.Unity.Resolver
{
    /// <summary>
    /// Extremely naive implementation of a dependency resolver, which finds
    /// types by name and creates a new instance each time.
    /// </summary>
    public class TypeScriptingDependencyResolver : IScriptDependencyResolver
    {
        /// <summary>
        /// Resolves a dependency by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Resolve(string name)
        {
            Type type = null;
            try
            {
                type = Type.GetType(name);
                return Activator.CreateInstance(type);
            }
            catch
            {
                //
            }

            return null;
        }
    }
}
