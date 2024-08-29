using System.Collections.Specialized;
using System.Linq;

using Avalonia.Collections;

using BMBot.GUI.Avalonia.Models.DataStructures.Logging;
using BMBot.GUI.Avalonia.Models.DataStructures.Logging.LogMessages;
using BMBot.GUI.Avalonia.Models.Enumerations.Logging;
using BMBot.GUI.Avalonia.Models.Extensions.Logging;
using BMBot.GUI.Avalonia.Models.Services.Game;
using BMBot.GUI.Avalonia.Views.Overlay;

using Microsoft.Extensions.Logging;

using Serilog.Events;

namespace BMBot.GUI.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly ILogger<MainWindowViewModel> i_logger;
    private readonly InstanceService             i_instanceService;

    public MainWindowViewModel(ILogger<MainWindowViewModel> p_logger,
                               InstanceService              p_instanceService)
    {
        i_logger               = p_logger;
        i_instanceService = p_instanceService;

        CollectionSink.SetCollection(LogMessages);
        
        LogMessages.CollectionChanged += (p_sender, p_args) =>
                                         {
                                             var newItem = p_args.NewItems?.OfType<IConsoleLogMessage>().LastOrDefault();

                                             if ( newItem is not null )
                                             {
                                                 switch ( newItem )
                                                 {
                                                     case ItemConsoleLogMessage itemConsoleLogMessage:
                                                         ItemLogMessages.Add(itemConsoleLogMessage);
                                                         break;
                                                     case GameConsoleLogMessage gameConsoleLogMessage:
                                                         GameLogMessages.Add(gameConsoleLogMessage);
                                                         break;
                                                     case MerchantConsoleLogMessage merchantConsoleLogMessage:
                                                         MerchantLogMessages.Add(merchantConsoleLogMessage);
                                                         break;
                                                 }

                                                 if ( newItem.LogLevel is LogEventLevel.Error )
                                                 {
                                                     ErrorLogMessages.Add(newItem);
                                                 }
                                             }
                                         };
        
        i_logger.LogInformation(LogMessageType.STANDARD, "An information log message for the standard log.");
        i_logger.LogWarning(LogMessageType.STANDARD, "A warning log message for the standard log.");
        i_logger.LogError(LogMessageType.STANDARD, "An error log message for the standard log.");
        i_logger.LogCritical(LogMessageType.STANDARD, "A critical log message for the standard log.");
        i_logger.LogTrace(LogMessageType.STANDARD, "A trace log message for the standard log.");
        i_logger.LogDebug(LogMessageType.STANDARD, "A debug log message for the standard log.");
        
        i_logger.LogInformation(LogMessageType.GAME, "An information log message for the game log.");
        i_logger.LogWarning(LogMessageType.GAME, "A warning log message for the game log.");
        i_logger.LogError(LogMessageType.GAME, "An error log message for the game log.");
        i_logger.LogCritical(LogMessageType.GAME, "A critical log message for the game log.");
        i_logger.LogTrace(LogMessageType.GAME, "A trace log message for the game log.");
        i_logger.LogDebug(LogMessageType.GAME, "A debug log message for the game log.");
        
        i_logger.LogInformation(LogMessageType.ITEM, "An information log message for the item log.");
        i_logger.LogWarning(LogMessageType.ITEM, "A warning log message for the item log.");
        i_logger.LogError(LogMessageType.ITEM, "An error log message for the item log.");
        i_logger.LogCritical(LogMessageType.ITEM, "A critical log message for the item log.");
        i_logger.LogTrace(LogMessageType.ITEM, "A trace log message for the item log.");
        i_logger.LogDebug(LogMessageType.ITEM, "A debug log message for the item log.");
        
        i_logger.LogInformation(LogMessageType.MERCHANT, "An information log message for the merchant log.");
        i_logger.LogWarning(LogMessageType.MERCHANT, "A warning log message for the merchant log.");
        i_logger.LogError(LogMessageType.MERCHANT, "An error log message for the merchant log.");
        i_logger.LogCritical(LogMessageType.MERCHANT, "A critical log message for the merchant log.");
        i_logger.LogTrace(LogMessageType.MERCHANT, "A trace log message for the merchant log.");
        i_logger.LogDebug(LogMessageType.MERCHANT, "A debug log message for the merchant log.");
        
        i_logger.LogInformation(LogMessageType.STANDARD, "An information log message for the standard log.");
        
        var instance = i_instanceService.Instances.First();
        
        var overlayView = new OverlayWindowView
                          {
                              DataContext = instance
                          };

        var overlayOptionsView = new OverlayOptionsView
                                 {
                                     DataContext = instance
                                 };

        instance.Window.SetMainWindowPositionAction = overlayView.SetPosition;
        instance.Window.SetOptionsWindowPositionAction = overlayOptionsView.SetPosition;
        instance.Window.CloseAction             += overlayView.Close;
        instance.Window.CloseAction             += overlayOptionsView.Close;
        
        instance.Window.RefreshWindowPositions();
        
        overlayView.Show();
    }
    
    public int SelectedPageIndex { get; set; }

    public AvaloniaList<IConsoleLogMessage> LogMessages         { get; } = [];
    public AvaloniaList<IConsoleLogMessage> SelectedLogMessages { get; } = [];

    public AvaloniaList<ItemConsoleLogMessage> ItemLogMessages         { get; } = [];
    public AvaloniaList<IConsoleLogMessage>    SelectedItemLogMessages { get; } = [];

    public AvaloniaList<GameConsoleLogMessage> GameLogMessages         { get; } = [];
    public AvaloniaList<IConsoleLogMessage>    SelectedGameLogMessages { get; } = [];

    public AvaloniaList<MerchantConsoleLogMessage> MerchantLogMessages         { get; } = [];
    public AvaloniaList<IConsoleLogMessage>        SelectedMerchantLogMessages { get; } = [];

    public AvaloniaList<IConsoleLogMessage> ErrorLogMessages         { get; } = [];
    public AvaloniaList<IConsoleLogMessage> SelectedErrorLogMessages { get; } = [];
}