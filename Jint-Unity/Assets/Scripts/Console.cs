using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public interface IConsoleDelegate
{
    void Execute(
        string command,
        IConsoleContext context,
        Action complete);
}

public interface IConsoleContext
{
    void Clear();
    void WriteLine(string line);
    void Write(string text);
}

public class DefaultConsoleContext : IConsoleContext
{
    private readonly Text _text;

    public DefaultConsoleContext(Text textfield)
    {
        _text = textfield;
    }

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

        _text.text += text;
    }

    /// <summary>
    /// Writes a line to the console.
    /// </summary>
    /// <param name="line"></param>
    public void WriteLine(string line)
    {
        if (!line.EndsWith('\n'.ToString()))
        {
            line = line + '\n';
        }

        Write(line);
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
    /// <summary>
    /// The context implementation.
    /// </summary>
    public IConsoleContext Context;

    /// <summary>
    /// Root of prompt. Always visible.
    /// </summary>
    public string Root;

    /// <summary>
    /// If true, echos commands to console.
    /// </summary>
    public bool EchoCommands;

    /// <summary>
    /// Stack of dirs which are combined into a prompt.
    /// </summary>
    private readonly Stack<string> _dirs = new Stack<string>();

    /// <summary>
    /// Prompt!
    /// </summary>
    private string _prompt;
    
    /// <summary>
    /// Buffer + index with which to accumulate input.
    /// </summary>
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

        if (null != Context)
        {
            Context.WriteLine(_prompt);
        }
    }

    /// <summary>
    /// Pops a dir from the stack.
    /// </summary>
    public void PopDir()
    {
        _dirs.Pop();

        BakePrompt();

        if (null != Context)
        {
            Context.WriteLine(_prompt);
        }
    }
    
    /// <summary>
    /// Clears prompt.
    /// </summary>
    public void Clear()
    {
        Context.Clear();
        Context.WriteLine(_prompt);
    }

    /// <summary>
    /// Called as early as possible in a MonoBehaviors lifecycle.
    /// </summary>
    private void Awake()
    {
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
                    Context.WriteLine(command);
                }

                if (null != CommandDelegate)
                {
                    CommandDelegate.Execute(
                        command,
                        Context,
                        CompleteCommand);
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
        Context.WriteLine(_prompt);
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