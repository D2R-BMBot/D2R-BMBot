using BMBot.GUI.Avalonia.Models.Enumerations.Navigation;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.Services.Navigation;

public class NavigationService : ReactiveObject
{
    [Reactive] public Workspace CurrentWorkspace { get; private set; }
    
    public void SetNavigation(Workspace p_workspace)
    {
        CurrentWorkspace = p_workspace;
    }
}