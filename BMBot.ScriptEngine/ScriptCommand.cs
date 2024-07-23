namespace ScriptEngine;

public class ScriptCommand(Func<Task> p_command, ActionPriority p_priority) : IActionItem
{
    public Func<Task> Command { get; } = p_command;
    public ActionPriority Priority { get; } = p_priority;
    
    public async Task Execute()
    {
        await Command();
    }
}