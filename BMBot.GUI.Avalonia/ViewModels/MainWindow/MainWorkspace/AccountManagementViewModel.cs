using Avalonia.Collections;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account;
using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;
using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Login;
using BMBot.GUI.Avalonia.Models.Enumerations.Logging;
using BMBot.GUI.Avalonia.Models.Extensions.Logging;

using Microsoft.Extensions.Logging;

using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.ViewModels.MainWindow.MainWorkspace;

public class AccountManagementViewModel(ILogger<AccountManagementViewModel> i_logger) : ViewModelBase
{
    public AvaloniaList<IAccountItem> TreeItems { get; } = [new GameAccount
                                                                {
                                                                    LoginMode = new CredentialLogin("My User Name", "My Password"),
                                                                    DisplayName = "Test Account 1",
                                                                    Characters =
                                                                    {
                                                                        new AmazonCharacter("Amazon 1"),
                                                                        new AssassinCharacter("Assassin 1"),
                                                                        new NecromancerCharacter("Necromancer 1"),
                                                                        new BarbarianCharacter("Barbarian 1"),
                                                                        new PaladinCharacter("Paladin 1"),
                                                                        new SorceressCharacter("Sorceress 1"),
                                                                        new DruidCharacter("Druid 1"),
                                                                    }
                                                                },
                                                               new GameAccount
                                                               {
                                                                   LoginMode = new TokenLogin("My Token"),
                                                                   DisplayName = "Test Account 2",
                                                                   Characters =
                                                                   {
                                                                       new AmazonCharacter("Amazon 2"),
                                                                       new AssassinCharacter("Assassin 2"),
                                                                       new NecromancerCharacter("Necromancer 2"),
                                                                       new BarbarianCharacter("Barbarian 2"),
                                                                       new PaladinCharacter("Paladin 2"),
                                                                       new SorceressCharacter("Sorceress 2"),
                                                                       new DruidCharacter("Druid 2"),
                                                                   }
                                                               }];
    
    [Reactive] public IAccountItem? SelectedItem { get; set; }

    public void ClickAddAccount()
    {
        i_logger.LogDebug(LogMessageType.STANDARD, "Add Account Clicked");
    }
    
    public void ClickRemoveAccount()
    {
        i_logger.LogDebug(LogMessageType.STANDARD, "Remove Account Clicked");
    }
    
    public void ClickAddCharacter()
    {
        i_logger.LogDebug(LogMessageType.STANDARD, "Add Character Clicked");
    }
    
    public void ClickRemoveCharacter()
    {
        i_logger.LogDebug(LogMessageType.STANDARD, "Remove Character Clicked");
    }
}