using System;
using System.Globalization;

using Avalonia.Data.Converters;

using BMBot.GUI.Avalonia.Models.DataStructures.UI.AccountData.Login;

namespace BMBot.GUI.Avalonia.Models.Converters;

public class LoginModeToIndexConverter : IValueConverter
{
    public object? Convert(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        if ( p_value is not ILoginMode loginMode ) return 0;

        return loginMode switch
               {
                   CredentialLogin => 0,
                   TokenLogin      => 1,
                   _                   => 0
               };
    }

    public object? ConvertBack(object? p_value, Type p_targetType, object? p_parameter, CultureInfo p_culture)
    {
        throw new NotImplementedException();
        // if ( p_value is not int indexValue ) return null;
        //
        // return indexValue switch
        //        {
        //            0 => new CredentialLogin(string.Empty, string.Empty),
        //            1 => new TokenLogin(),
        //            _ => null
        //        };
    }
}