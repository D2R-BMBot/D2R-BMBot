using BMBot.GUI.Avalonia.Models.Enumerations.Logging;

using Microsoft.Extensions.Logging;

namespace BMBot.GUI.Avalonia.Models.Extensions.Logging;

public static class LoggingExtensions
{
    public static void LogInformation(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogInformation("[{Type}]{Message}", p_messageType, p_message);
    }
    
    public static void LogWarning(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogWarning("[{Type}]{Message}", p_messageType, p_message);
    }
    
    public static void LogError(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogError("[{Type}]{Message}", p_messageType, p_message);
    }
    
    public static void LogCritical(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogCritical("[{Type}]{Message}", p_messageType, p_message);
    }
    
    public static void LogTrace(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogTrace("[{Type}]{Message}", p_messageType, p_message);
    }
    
    public static void LogDebug(this ILogger p_logger, LogMessageType p_messageType, string p_message)
    {
        p_logger.LogDebug("[{Type}]{Message}", p_messageType, p_message);
    }
}