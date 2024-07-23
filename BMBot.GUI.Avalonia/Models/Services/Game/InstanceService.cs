using System.Collections.Generic;
using System.Diagnostics;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

using Microsoft.Extensions.Logging;

namespace BMBot.GUI.Avalonia.Models.Services.Game;

public class InstanceService
{
    public InstanceService()
    {
        GetInstances();
    }
    
    public List<InstanceData> Instances { get; } = [];

    public void GetInstances()
    {
        var d2rProcesses = Process.GetProcessesByName("D2R");
        
        foreach (var process in d2rProcesses)
        {
            if ( process.MainModule is null ) continue;
            
            var instanceData = new InstanceData(process);

            Instances.Add(instanceData);
        }
    }
}