using System;
using System.Globalization;

using Avalonia.Data.Converters;

using BMBot.GUI.Avalonia.Models.DataStructures.Logging.LogMessages;

using Material.Icons;

using Serilog.Events;

namespace BMBot.GUI.Avalonia.Models.Converters;

public class ConsoleLogMessageToMaterialIconKindConverter : IValueConverter
{
    public object? Convert(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        if ( p_value is not IConsoleLogMessage logMessage ) return MaterialIconKind.Abacus;

        return logMessage switch
               {
                   StandardConsoleLogMessage => MaterialIconKind.FileDocument,
                   ItemConsoleLogMessage     => MaterialIconKind.Shield,
                   GameConsoleLogMessage     => MaterialIconKind.Games,
                   MerchantConsoleLogMessage => MaterialIconKind.AttachMoney,
                   _                         => MaterialIconKind.Abacus
               };
    }

    public object? ConvertBack(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        throw new NotImplementedException();
    }
}