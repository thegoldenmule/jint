namespace TheGoldenMule
{
    /// <summary>
    /// Defines an interface for a console.
    /// </summary>
    public interface IConsoleExecutionContext
    {
        /// <summary>
        /// Clears console.
        /// </summary>
        void Clear();

        /// <summary>
        /// Writes a line to the console.
        /// </summary>
        /// <param name="line"></param>
        void WriteLine(string line);

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="text"></param>
        void Write(string text);
    }
}