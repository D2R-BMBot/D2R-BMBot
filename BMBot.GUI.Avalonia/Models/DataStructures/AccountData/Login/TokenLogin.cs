namespace BMBot.GUI.Avalonia.Models.DataStructures.UI.AccountData.Login;

public class TokenLogin(string p_token) : ILoginMode
{
    public string Token { get; set; } = p_token;
}