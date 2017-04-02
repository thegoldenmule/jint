using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public interface IConsoleDelegate
{
    void Execute(
        string command,
        IConsoleContext context);
}

public interface IConsoleContext
{
    void WriteLine(string line);
    void Complete();
}

public class DefaultConsoleContext : IConsoleContext
{
    private readonly Action<string> _writeLine;
    private readonly Action _complete; 

    public DefaultConsoleContext(
        Action<string> writeLine,
        Action complete)
    {
        _writeLine = writeLine;
        _complete = complete;
    }

    public void WriteLine(string line)
    {
        _writeLine(line);
    }

    public void Complete()
    {
        _complete();
    }
}

/// <summary>
/// Very basic, very consistent console implementation. The only interface is
/// for listening for complete commands to be input. For consistency, and
/// compatibility with asynchronous commands, there is no public interface for
/// writing to the Console. This is so that the prompt can be guaranteed to be
/// correct.
/// 
/// Usage:
/// 
/// console.CommandDelegate = this;
/// 
/// ...
/// 
/// public void Execute(
///        string command,
///        IConsoleContext context)
/// {
///     context.WriteLine('Performing commands!');
///     ... perform actions, may be asynchronous
///     ... once finished, complete
///     context.Complete();
///     
///     // Now, because the Console knows the command has been completed, it
///     // can write the prompt with the knowledge that nothing will be able to
///     // be written after the prompt.
/// }
/// </summary>
public class Console : MonoBehaviour
{
    public Text TextField;
    public string Root;
    public bool EchoCommands;

    private readonly Stack<string> _dirs = new Stack<string>();
    private string _prompt;
    
    private DefaultConsoleContext _context;

    private char[] _accumBuffer = new char[255];
    private int _accumBufferIndex = 0;

    /// <summary>
    /// Instead of using an event, which allows multiple subscribers, enforce
    /// only one listener, so we can make command flow consistent.
    /// </summary>
    public IConsoleDelegate CommandDelegate; 

    /// <summary>
    /// Pushes a dir onto the stack.
    /// </summary>
    /// <param name="dir"></param>
    public void PushDir(string dir)
    {
        _dirs.Push(dir);

        BakePrompt();
        WriteLine(_prompt);
    }

    /// <summary>
    /// Pops a dir from the stack.
    /// </summary>
    public void PopDir()
    {
        _dirs.Pop();

        BakePrompt();
    }
    
    /// <summary>
    /// Clears prompt.
    /// </summary>
    public void Clear()
    {
        if (null == TextField)
        {
            return;
        }

        TextField.text = string.Empty;
        WriteLine(_prompt);
    }

    /// <summary>
    /// Called as early as possible in a MonoBehaviors lifecycle.
    /// </summary>
    private void Awake()
    {
        _context = new DefaultConsoleContext(WriteLine, CompleteCommand);

        BakePrompt();
    }

    /// <summary>
    /// Called late to read in commands.
    /// </summary>
    private void LateUpdate()
    {
        var input = Input.inputString.ToCharArray();
        for (int i = 0, len = input.Length; i < len; i++)
        {
            // process command
            if (input[i] == '\n')
            {
                var commandBuffer = new char[_accumBufferIndex];
                Array.Copy(_accumBuffer, commandBuffer, _accumBufferIndex);
                var command = commandBuffer.ToString();

                // reset state before we do anything
                _accumBufferIndex = 0;

                if (EchoCommands)
                {
                    WriteLine(command);
                }

                if (null != CommandDelegate)
                {
                    CommandDelegate.Execute(
                        command,
                        _context);
                }
            }
            else
            {
                // see if we need to grow the buffer
                if (_accumBufferIndex == _accumBuffer.Length)
                {
                    var buffer = new char[_accumBuffer.Length * 2];
                    Array.Copy(_accumBuffer, 0, buffer, 0, _accumBuffer.Length);
                    _accumBuffer = buffer;
                }

                _accumBuffer[_accumBufferIndex++] = input[i];
            }
        }
    }

    /// <summary>
    /// Called by listener 
    /// </summary>
    private void CompleteCommand()
    {
        WriteLine(_prompt);
    }

    /// <summary>
    /// Writes a line to the console.
    /// </summary>
    /// <param name="line"></param>
    private void WriteLine(string line)
    {
        if (null == TextField)
        {
            return;
        }

        TextField.text += line;
    }

    /// <summary>
    /// Bake dir stack into a prompt.
    /// </summary>
    private void BakePrompt()
    {
        _prompt = (string.IsNullOrEmpty(Root) ? "" : Root + "/")
                  + string.Join("/", _dirs.ToArray())
                  + "> ";
    }
}