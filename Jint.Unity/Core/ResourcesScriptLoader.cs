using UnityEngine;

namespace Jint.Unity
{
    /// <summary>
    /// IScriptLoader that pulls from Resources.
    /// </summary>
    public class ResourcesScriptLoader : IScriptLoader
    {
        /// <summary>
        /// Loads a script!
        /// </summary>
        /// <param name="path"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public bool Load(string path, out string script)
        {
            script = string.Empty;

            var resource = Resources.Load<TextAsset>(path);
            if (null == resource)
            {
                return false;
            }

            script = resource.text;
            return true;
        }
    }
}