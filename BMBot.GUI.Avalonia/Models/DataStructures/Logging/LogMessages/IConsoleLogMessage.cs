using Serilog.Events;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Logging.LogMessages;

public interface IConsoleLogMessage
{
    public LogEventLevel LogLevel { get; }
    public string        Message     { get; }
}