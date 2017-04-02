using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TheGoldenMule
{
    /// <summary>
    /// Very basic, but _consistent_ console implementation.
    /// 
    /// Usage:
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
        /// Unity text field.
        /// </summary>
        public Text Text;

        /// <summary>
        /// Root of prompt. Always visible.
        /// </summary>
        public string Root;

        /// <summary>
        /// If true, echos commands to console.
        /// </summary>
        public bool EchoCommands;

        /// <summary>
        /// An implementation of IConsoleExecutionContext, which provides an api
        /// for commands.
        /// </summary>
        private IConsoleExecutionContext _executionContext;

        /// <summary>
        /// Instead of using an event, which allows multiple subscribers, enforce
        /// only one listener, so we can make command flow consistent.
        /// </summary>
        private IConsoleExecutionDelegate _executionDelegate;

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
        /// Indices into full block of text.
        /// </summary>
        private int _promptStartIndex;
        private int _promptEndIndex;

        /// <summary>
        /// Called as early as possible in a MonoBehaviors lifecycle.
        /// </summary>
        public void Init(
            IConsoleExecutionDelegate @delegate,
            IConsoleExecutionContext context)
        {
            _executionDelegate = @delegate;
            _executionContext = context;

            BakePrompt();

            Text.text = string.Empty;

            InitPrompt();
        }

        /// <summary>
        /// Pushes a dir onto the stack.
        /// </summary>
        /// <param name="dir"></param>
        public void PushDir(string dir)
        {
            _dirs.Push(dir);

            BakePrompt();

            if (null != _executionContext)
            {
                _executionContext.WriteLine(_prompt);
            }
        }

        /// <summary>
        /// Pops a dir from the stack.
        /// </summary>
        public void PopDir()
        {
            _dirs.Pop();

            BakePrompt();

            if (null != _executionContext)
            {
                _executionContext.WriteLine(_prompt);
            }
        }
    
        /// <summary>
        /// Clears prompt.
        /// </summary>
        public void Clear()
        {
            Text.text = string.Empty;
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
                else if (input[i] == '\b')
                {
                    _accumBufferIndex = Mathf.Max(0, _accumBufferIndex - 1);
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

            if (input.Length > 0)
            {
                UpdatePromptText();
            }

            // process command
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
            var command = new string(commandBuffer);

            // reset state before we execute
            _accumBufferIndex = 0;

            // remove prompt from textfield
            var text = Text.text.Substring(0, _promptStartIndex);

            // write command to console if desired
            if (EchoCommands)
            {
                text += command + "\n";
            }

            // update text
            Text.text = text;

            // hand execution over to the delegate
            if (null != _executionDelegate)
            {
                _executionDelegate.Execute(
                    command,
                    _executionContext,
                    CompleteCommand);
            }
            else
            {
                CompleteCommand();
            }
        }

        /// <summary>
        /// Called by ExecutionDelegate when the command is finished.
        /// </summary>
        private void CompleteCommand()
        {
            if (!Text.text.EndsWith("\n"))
            {
                Text.text += "\n";
            }

            InitPrompt();
        }

        /// <summary>
        /// Writes the prompt to the textfield again.
        /// </summary>
        private void InitPrompt()
        {
            _promptStartIndex = Text.text.Length;
            Text.text += _prompt;
            _promptEndIndex = Text.text.Length;
        }

        /// <summary>
        /// Writes all prompt text.
        /// </summary>
        private void UpdatePromptText()
        {
            // back up
            var text = Text.text.Substring(0, _promptEndIndex);

            // write accumulator
            var commandBuffer = new char[_accumBufferIndex];
            Array.Copy(_accumBuffer, commandBuffer, _accumBufferIndex);

            text += new string(commandBuffer);
            Text.text = text;
        }

        /// <summary>
        /// Bake dir stack into a prompt.
        /// </summary>
        private void BakePrompt()
        {
            _prompt = (string.IsNullOrEmpty(Root) ? "" : Root)
                + (_dirs.Count > 0 ? "/" + string.Join("/", _dirs.ToArray()) : string.Empty)
                + "> ";
        }
    }
}