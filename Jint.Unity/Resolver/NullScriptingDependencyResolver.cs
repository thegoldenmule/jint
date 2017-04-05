namespace Jint.Unity
{
    /// <summary>
    /// Implementation that always returns null.
    /// </summary>
    public class NullScriptingDependencyResolver : IScriptDependencyResolver
    {
        /// <summary>
        /// Always returns null.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object Resolve(string name)
        {
            return null;
        }
    }
}