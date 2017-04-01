using Jint;

namespace JintUnity
{
    /// <summary>
    /// Hosts scripts and provides a default Unity API.
    /// </summary>
    public class UnityScriptingHost : Engine
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UnityScriptingHost()
        {
            SetValue("Log", new UnityLogWrapper());
            SetValue("Scene", new UnitySceneManager());
        }
    }
}