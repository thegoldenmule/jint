using System;
using TheGoldenMule;
using UnityEngine;
using UnityEngine.UI;
using Console = TheGoldenMule.Console;

namespace JintUnity
{
    public class JintRepl : MonoBehaviour, IConsoleExecutionDelegate
    {
        public Console Console;

        private readonly UnityScriptingHost _host = new UnityScriptingHost();

        private void Awake()
        {
            if (null == Console || null == Console.Text)
            {
                Debug.LogWarning("Cannot initialize JintRepl: must have Console and Console must have Text!");
                return;
            }

            Console.Init(this, new DefaultConsoleExecutionContext(Console.Text));
        }

        public void Execute(
            string command,
            IConsoleExecutionContext context,
            Action complete)
        {
            _host.Execute(command);
            context.WriteLine(_host.GetCompletionValue().ToString());

            complete();
        }
    }
}