using BMBot.GUI.Avalonia.Models.Services.Game;

namespace BMBot.Gui.Avalonia.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var instanceService = new InstanceService();
        
        instanceService.GetInstances();
    }
}