using System.IO;

using Avalonia.Collections;
using Avalonia.Controls;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;
using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Login;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Account;

public class GameAccount : ReactiveObject, IAccountItem
{
    [Reactive] public string     DisplayName   { get; set; }
    [Reactive] public ILoginMode LoginMode     { get; set; }
    [Reactive] public bool       AccountIsBusy { get; set; }

    public AvaloniaList<ICharacter> Characters                     { get; } = [];

    public void OnLoginModeChanged(SelectionChangedEventArgs p_args)
    {
        if ( p_args.Source is not ComboBox comboBox ) return;

        switch ( comboBox.SelectedIndex )
        {
            case 0:
                if ( LoginMode is CredentialLogin ) return;
                LoginMode = new CredentialLogin(string.Empty, string.Empty);
                break;
            case 1:
                if ( LoginMode is TokenLogin ) return;
                LoginMode = new TokenLogin(string.Empty);
                break;
            default:
                throw new InvalidDataException();
        }
    }
}