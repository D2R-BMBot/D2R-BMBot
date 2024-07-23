namespace ScriptEngine;

public class Script: IActionItem
{
    public Queue<ScriptCommand> Commands { get; set; } = [];
}