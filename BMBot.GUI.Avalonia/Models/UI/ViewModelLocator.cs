using System;

using BMBot.GUI.Avalonia.ViewModels;
using BMBot.GUI.Avalonia.ViewModels.MainWindow;
using BMBot.GUI.Avalonia.ViewModels.MainWindow.MainWorkspace;

using Microsoft.Extensions.DependencyInjection;

namespace BMBot.GUI.Avalonia.Models.UI;

public class ViewModelLocator(IServiceProvider p_serviceProvider)
{
    public MainWindowViewModel    MainWindowViewModel    => p_serviceProvider.GetRequiredService<MainWindowViewModel>();
    public MainWorkspaceViewModel MainWorkspaceViewModel => p_serviceProvider.GetRequiredService<MainWorkspaceViewModel>();
    public AccountManagementViewModel AccountManagementViewModel => p_serviceProvider.GetRequiredService<AccountManagementViewModel>();
}