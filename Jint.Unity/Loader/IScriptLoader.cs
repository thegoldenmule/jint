namespace Jint.Unity
{
    /// <summary>
    /// Describes an interface for loading scripts, synchronously.
    /// </summary>
    public interface IScriptLoader
    {
        /// <summary>
        /// Loads a script!
        /// </summary>
        /// <param name="path"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        bool Load(string path, out string script);
    }
}