using System;
using JintUnity;
using UnityEngine;

public class JintRepl : MonoBehaviour, IConsoleDelegate
{
    public Console Console;

    private readonly UnityScriptingHost _host = new UnityScriptingHost();

    private void Awake()
    {
        if (null == Console)
        {
            return;
        }
    }

    public void Execute(string command, IConsoleContext context)
    {
        //
    }
}