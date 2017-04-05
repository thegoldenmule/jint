using System;
using UnityEngine;

namespace Jint.Unity
{
    /// <summary>
    /// A serializable reference to a script.
    /// </summary>
    [Serializable]
    public class ScriptReference
    {
        /// <summary>
        /// Reference to a TextAsset. This is used over Contents if available.
        /// </summary>
        public TextAsset Asset;

        /// <summary>
        /// String contents if Asset is null.
        /// </summary>
        public string Contents;

        /// <summary>
        /// Retrieves the script as a string. Asset is prefered if not null.
        /// </summary>
        /// <returns></returns>
        public string String()
        {
            if (null == Asset)
            {
                return Contents ?? string.Empty;
            }

            return Asset.text;
        }
    }
}