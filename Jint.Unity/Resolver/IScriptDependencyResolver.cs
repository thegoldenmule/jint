namespace Jint.Unity
{
    /// <summary>
    /// Defines a way to resolve dependencies for a script.
    /// </summary>
    public interface IScriptDependencyResolver
    {
        /// <summary>
        /// Resolves a dependency by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        object Resolve(string name);
    }
}