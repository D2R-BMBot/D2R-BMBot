using System;
using System.Globalization;

using Avalonia.Data.Converters;

using Serilog.Events;

namespace BMBot.GUI.Avalonia.Models.Converters;

public class LogLevelToIndicatorVisibilityConverter : IValueConverter
{
    public object Convert(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        if ( p_value is not LogEventLevel logLevel ) return false;
        if ( p_parameter is not string stringParameter ) return false;

        return stringParameter switch
               {
                   "ERROR" => logLevel switch
                              {
                                  LogEventLevel.Error => true,
                                  _                   => false
                              },
                   "CRITICAL" => logLevel switch
                                 {
                                     LogEventLevel.Fatal => true,
                                     _                   => false
                                 },
                   "DEBUG" => logLevel switch
                              {
                                  LogEventLevel.Debug => true,
                                  _                   => false
                              },
                   _ => false
               };
    }

    public object ConvertBack(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        throw new NotImplementedException();
    }
}