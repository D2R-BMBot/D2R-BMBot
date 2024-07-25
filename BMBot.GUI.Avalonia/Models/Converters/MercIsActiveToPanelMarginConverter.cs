using System;
using System.Globalization;

using Avalonia;
using Avalonia.Data.Converters;

namespace BMBot.GUI.Avalonia.Models.Converters;

public class MercIsActiveToPanelMarginConverter : IValueConverter
{
    public object? Convert(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        if ( p_value is not bool boolValue ) return new Thickness(0);

        return boolValue ? new Thickness(0, 64, 0, 0) : new Thickness(0, 0, 0, 0);
    }

    public object? ConvertBack(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        throw new NotImplementedException();
    }
}