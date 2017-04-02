using System;
using JintUnity;
using UnityEngine;
using UnityEngine.UI;

public class JintRepl : MonoBehaviour, IConsoleExecutionDelegate
{
    public Text Textfield;
    public Console Console;

    private readonly UnityScriptingHost _host = new UnityScriptingHost();

    private void Awake()
    {
        if (null == Console || null == Textfield)
        {
            Debug.LogWarning("Cannot initialize JintRepl: must have Textfield and Console!");
            return;
        }

        Console.ExecutionContext = new DefaultConsoleExecutionContext(Textfield);
        Console.ExecutionDelegate = this;
    }

    public void Execute(
        string command,
        IConsoleExecutionContext context,
        Action complete)
    {
        Debug.Log("Execute : " + command);
    }
}