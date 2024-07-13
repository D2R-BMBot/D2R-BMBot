using System;
using System.Globalization;

using Avalonia.Data.Converters;
using Avalonia.Media;

using BMBot.GUI.Avalonia.Models.DataStructures.Logging.LogMessages;

using Serilog.Events;

namespace BMBot.GUI.Avalonia.Models.Converters;

public class ConsoleLogMessageToMaterialIconColorConverter : IValueConverter
{
    public object? Convert(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        if ( p_value is not IConsoleLogMessage logMessage ) return Brushes.Magenta;
        
        return logMessage switch
               {
                   StandardConsoleLogMessage => Brushes.DimGray,
                   ItemConsoleLogMessage     => Brushes.DodgerBlue,
                   GameConsoleLogMessage     => Brushes.Teal,
                   MerchantConsoleLogMessage => Brushes.DarkGoldenrod,
                   _                         => Brushes.Magenta
               };
    }

    public object? ConvertBack(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        throw new NotImplementedException();
    }
}