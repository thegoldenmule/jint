using System;

namespace Jint.Ninject
{
    /// <summary>
    /// Allows a class to be given an easy name to reference in JS.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NinjectScriptDependencyAttribute : Attribute
    {
        /// <summary>
        /// Name of the dependency.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public NinjectScriptDependencyAttribute(string name)
        {
            Name = name;
        }
    }
}