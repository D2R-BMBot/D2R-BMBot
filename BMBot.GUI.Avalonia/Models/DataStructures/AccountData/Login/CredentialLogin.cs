using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.UI.AccountData.Login;

public class CredentialLogin(string p_userName, string p_password) : ReactiveObject, ILoginMode
{
    [Reactive] public string UserName { get; set; } = p_userName;
    [Reactive] public string Password { get; set; } = p_password;
}