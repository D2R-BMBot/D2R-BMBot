using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

public class BotLoader
{
    private static BotLoader _instance;
    private Form1 form1;

    public List<IBot> Scripts { get; private set; } = new List<IBot>();
    public Dictionary<string, bool> ScriptCheckedStates { get; private set; } = new Dictionary<string, bool>();
    private List<string> blacklist = new List<string> { "ChaosLeech.cs", "ignore_this_too.cs" };

    private BotLoader(Form1 form1)
    {
        this.form1 = form1;
        LoadScripts();
        InitializeScriptCheckedStates();
    }

    public static BotLoader GetInstance(Form1 form1)
    {
        if (_instance == null)
        {
            _instance = new BotLoader(form1);
        }
        return _instance;
    }

    private void InitializeScriptCheckedStates()
    {
        foreach (var script in Scripts)
        {
            string scriptFileName = GetFileName(script);
            if (!ScriptCheckedStates.ContainsKey(scriptFileName))
            {
                ScriptCheckedStates[scriptFileName] = false;
            }
        }
    }

    public void LoadScripts()
    {
        Scripts.Clear();
        var assembly = Assembly.GetExecutingAssembly();
        var botTypes = assembly.GetTypes().Where(t => typeof(IBot).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract).ToArray();

        foreach (var type in botTypes)
        {
            var scriptInstance = (IBot)Activator.CreateInstance(type);
            if (!blacklist.Contains(type.Name + ".cs"))
            {
                Scripts.Add(scriptInstance);
            }
        }

        InitializeScriptCheckedStates();
    }

    public string GetFileName(IBot script)
    {
        return script.GetType().Name + ".cs";
    }

    public void UpdateScriptCheckedState(string scriptFileName, bool isChecked)
    {
        if (ScriptCheckedStates.ContainsKey(scriptFileName))
        {
            ScriptCheckedStates[scriptFileName] = isChecked;
        }
    }
}
