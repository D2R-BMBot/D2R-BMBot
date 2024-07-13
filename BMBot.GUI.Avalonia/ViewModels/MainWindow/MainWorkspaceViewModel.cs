using System;

using BMBot.GUI.Avalonia.Models.Enumerations.Logging;
using BMBot.GUI.Avalonia.Models.Enumerations.Navigation;
using BMBot.GUI.Avalonia.Models.Extensions.Logging;
using BMBot.GUI.Avalonia.Models.Services.Navigation;

using Microsoft.Extensions.Logging;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.ViewModels.MainWindow;

public class MainWorkspaceViewModel : ViewModelBase
{
    private readonly ILogger<MainWorkspaceViewModel> i_logger;
    private readonly NavigationService               i_navigationService;

    public MainWorkspaceViewModel(ILogger<MainWorkspaceViewModel> p_logger,
                                  NavigationService               p_navigationService)
    {
        i_logger            = p_logger;
        i_navigationService = p_navigationService;

        i_navigationService.WhenAnyValue(p_service => p_service.CurrentWorkspace)
                           .Subscribe(OnNavigationChanged);
    }

    [Reactive] public bool   PaneIsOpen             { get; set; }
    [Reactive] public string WorkspaceTitle         { get; set; } = string.Empty;
    [Reactive] public int    SelectedWorkspaceIndex { get; set; }

    public void ClickExpandNavigationPanel()
    {
        PaneIsOpen = !PaneIsOpen;
    }

    public void ClickNavigationButton(object? p_parameter)
    {
        if ( p_parameter is not string navigationTarget ) return;

        var workspace = navigationTarget switch
                        {
                            "DEPLOY"   => Workspace.DEPLOY,
                            "ACCOUNT"  => Workspace.ACCOUNT_MANAGEMENT,
                            "SCRIPT"   => Workspace.SCRIPT_MANAGEMENT,
                            "SETTINGS" => Workspace.SETTINGS,
                            _          => Workspace.DEPLOY
                        };

        i_navigationService.SetNavigation(workspace);
    }

    private void OnNavigationChanged(Workspace p_workspace)
    {
        WorkspaceTitle = p_workspace switch
                         {
                             Workspace.DEPLOY             => "Deploy",
                             Workspace.ACCOUNT_MANAGEMENT => "Account Management",
                             Workspace.SCRIPT_MANAGEMENT  => "Script Management",
                             Workspace.SETTINGS           => "Settings",
                             _                            => string.Empty
                         };
        
        SelectedWorkspaceIndex = (int) p_workspace;
    }
}