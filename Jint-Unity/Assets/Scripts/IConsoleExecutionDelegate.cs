using System;

/// <summary>
/// Defines a delegate for handling console commands.
/// </summary>
public interface IConsoleExecutionDelegate
{
    /// <summary>
    /// Executes a command. During the command, the IConsoleExecutionContext is
    /// used to interface with the console. After the command is complete, the
    /// complete Action must be used to end the command.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="context"></param>
    /// <param name="complete"></param>
    void Execute(
        string command,
        IConsoleExecutionContext context,
        Action complete);
}