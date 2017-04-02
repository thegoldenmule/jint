using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Very basic, but _consistent_ console implementation.
/// 
/// Usage:
/// 
/// // First, define a context. This object provides an interface for commands
/// // to affect the console.
/// console.ExecutionContext = new DefaultConsoleExecutionContext(myTextField);
/// 
/// // Then, set an IConsoleExecutionDelegate-- an object that parses + runs
/// // commands.
/// console.ExecutionDelegate = this;
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
    /// An implementation of IConsoleExecutionContext, which provides an api
    /// for commands.
    /// </summary>
    public IConsoleExecutionContext ExecutionContext;

    /// <summary>
    /// Instead of using an event, which allows multiple subscribers, enforce
    /// only one listener, so we can make command flow consistent.
    /// </summary>
    public IConsoleExecutionDelegate ExecutionDelegate; 

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
    /// Pushes a dir onto the stack.
    /// </summary>
    /// <param name="dir"></param>
    public void PushDir(string dir)
    {
        _dirs.Push(dir);

        BakePrompt();

        if (null != ExecutionContext)
        {
            ExecutionContext.WriteLine(_prompt);
        }
    }

    /// <summary>
    /// Pops a dir from the stack.
    /// </summary>
    public void PopDir()
    {
        _dirs.Pop();

        BakePrompt();

        if (null != ExecutionContext)
        {
            ExecutionContext.WriteLine(_prompt);
        }
    }
    
    /// <summary>
    /// Clears prompt.
    /// </summary>
    public void Clear()
    {
        ExecutionContext.Clear();
        ExecutionContext.WriteLine(_prompt);
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
                ProcessCommand();
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

                ExecutionContext.Write(input[i].ToString());
                _accumBuffer[_accumBufferIndex++] = input[i];
            }
        }

        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            ProcessCommand();
        }
    }

    /// <summary>
    /// Called to execute a command.
    /// </summary>
    private void ProcessCommand()
    {
        var commandBuffer = new char[_accumBufferIndex];
        Array.Copy(_accumBuffer, commandBuffer, _accumBufferIndex);
        var command = commandBuffer.ToString();

        // reset state before we do anything
        _accumBufferIndex = 0;

        if (EchoCommands)
        {
            ExecutionContext.WriteLine(command);
        }

        if (null != ExecutionDelegate)
        {
            ExecutionDelegate.Execute(
                command,
                ExecutionContext,
                CompleteCommand);
        }
    }

    /// <summary>
    /// Called by listener when the command is finished.
    /// </summary>
    private void CompleteCommand()
    {
        ExecutionContext.WriteLine(_prompt);
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