namespace Jint.Unity
{
    /// <summary>
    /// Wraps Unity logging interface for Jint.
    /// </summary>
    public class UnityLogWrapper
    {
        /// <summary>
        /// Debug level logging.
        /// </summary>
        public void Debug(object message, params object[] replacements)
        {
            UnityEngine.Debug.Log(string.Format(message.ToString(), replacements));
        }

        /// <summary>
        /// Warn level logging.
        /// </summary>
        public void Warn(object message, params object[] replacements)
        {
            UnityEngine.Debug.LogWarning(string.Format(message.ToString(), replacements));
        }

        /// <summary>
        /// Error level logging.
        /// </summary>
        public void Error(object message, params object[] replacements)
        {
            UnityEngine.Debug.LogError(string.Format(message.ToString(), replacements));
        }

        /// <summary>
        /// Assert level logging.
        /// </summary>
        public void Assert(object message, params object[] replacements)
        {
            UnityEngine.Debug.LogAssertion(string.Format(message.ToString(), replacements));
        }
    }
}
