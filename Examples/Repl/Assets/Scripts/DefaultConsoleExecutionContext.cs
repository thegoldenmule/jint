using UnityEngine.UI;

namespace TheGoldenMule
{
    /// <summary>
    /// A default implementation of IConsoleExecutionContext. This uses a simple
    /// textfield.
    /// </summary>
    public class DefaultConsoleExecutionContext : IConsoleExecutionContext
    {
        /// <summary>
        /// Unity Text component.
        /// </summary>
        private readonly Text _text;

        /// <summary>
        /// Creates a console context.
        /// </summary>
        /// <param name="textfield"></param>
        public DefaultConsoleExecutionContext(Text textfield)
        {
            _text = textfield;
        }

        /// <summary>
        /// Clears text.
        /// </summary>
        public void Clear()
        {
            if (null != _text)
            {
                _text.text = string.Empty;
            }
        }

        /// <summary>
        /// Writes test to the console.
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            if (null == _text)
            {
                return;
            }

            _text.text += text ?? string.Empty;
        }

        /// <summary>
        /// Writes a line to the console.
        /// </summary>
        /// <param name="line"></param>
        public void WriteLine(string line)
        {
            line = line ?? string.Empty;

            if (!line.EndsWith('\n'.ToString()))
            {
                line = line + '\n';
            }

            Write(line);
        }


    }
}