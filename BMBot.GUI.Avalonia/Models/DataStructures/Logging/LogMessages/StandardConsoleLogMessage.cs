using Serilog.Events;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Logging.LogMessages;

public class StandardConsoleLogMessage(LogEventLevel p_logEventLevel, string p_message) : IConsoleLogMessage
{
    public LogEventLevel LogLevel { get; } = p_logEventLevel;
    public string        Message  { get; } = p_message;
}