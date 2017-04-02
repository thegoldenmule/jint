using System;
using TheGoldenMule;
using UnityEngine;
using Console = TheGoldenMule.Console;

namespace JintUnity
{
    /// <summary>
    /// A very basic REPL.
    /// </summary>
    public class JintRepl : MonoBehaviour, IConsoleExecutionDelegate
    {
        /// <summary>
        /// The Console to use.
        /// </summary>
        public Console Console;

        /// <summary>
        /// Scripting host to use.
        /// </summary>
        private readonly UnityScriptingHost _host = new UnityScriptingHost();

        /// <summary>
        /// Called to initialize.
        /// </summary>
        private void Awake()
        {
            if (null == Console || null == Console.Text)
            {
                Debug.LogWarning("Cannot initialize JintRepl: must have Console and Console must have Text!");
                return;
            }

            var executionContext = new DefaultConsoleExecutionContext(Console.Text);
            Console.Init(this, executionContext);

            _host.SetValue("clear", new Action(Console.Clear));
        }

        /// <summary>
        /// IConsoleExecutionDelegate implementation. Called when a command has
        /// been executed.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="complete"></param>
        public void Execute(
            string command,
            IConsoleExecutionContext context,
            Action complete)
        {
            try
            {
                _host.Execute(command);
                context.WriteLine(" => " + _host.GetCompletionValue());
            }
            catch (Exception exception)
            {
                context.WriteLine(" => " + exception.Message);
            }

            context.WriteLine(null);
            complete();
        }
    }
}