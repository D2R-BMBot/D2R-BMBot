namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Login;

public class TokenLogin(string p_token) : ILoginMode
{
    public string Token { get; set; } = p_token;
}