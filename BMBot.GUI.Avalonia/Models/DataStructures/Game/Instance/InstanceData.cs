using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.InteropServices;

using Avalonia.Input;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;
using BMBot.GUI.Avalonia.Models.Services.Interop.Memory;
using BMBot.Interop.API.Process;
using BMBot.Interop.API.Window;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class InstanceData
{
    public InstanceData(Process p_applicationProcess)
    {
        if ( p_applicationProcess.MainModule is null )
        {
            throw new InvalidDataException();
        }

        Window    = new WindowData(p_applicationProcess);
        ProcessHandle = ProcessInterop.OpenProcess((int)ProcessAccess.PROCESS_QUERY_INFORMATION | (int)ProcessAccess.PROCESS_VM_READ, false, p_applicationProcess.Id);
        
        Pointers.BaseAddress = p_applicationProcess.MainModule.BaseAddress;
        
        FillPlayerPointers();

        Character = new BarbarianCharacter("Testo_Two");
        
        var refreshTimer = Observable.Interval(TimeSpan.FromSeconds(0.5));
        
        refreshTimer.Subscribe(_ =>
        {
            if ( !Pointers.PointersAreInitialized ||
                 !Pointers.PlayerPointersAreInitialized )
            {
                return;
            }
            
            FillPlayerPointers();

            if ( Character is not null )
            {
                Character.XPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionXAddress);
                Character.YPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionYAddress);
            }
        });
    }
    
    public IntPtr ProcessHandle { get; }
    
    public InstancePointers Pointers { get; } = new();
    
    public WindowData Window { get; }
    public ICharacter Character  { get; private set; }
    
    public bool PlayerIsInGame()
    {
        if ( !Pointers.PointersAreInitialized ) return false;

        return GameMemoryService.ReadByte(this, Pointers.PlayerIsInGameAddress) == 0x1;
    }

    public void FillPlayerPointers()
    {
        if ( !Pointers.PointersAreInitialized ) return;
        
        const int arraySize = ( 128 + 516 ) * 8;

        var unitTableBuffer = GameMemoryService.GetMemorySpan(this, Pointers.UnitTableAddress, arraySize);

        for ( var i = 0; i < unitTableBuffer.Length; i += 8 )
        {
            var baseUnitAddress = Pointers.UnitTableAddress + i;
            var unitPointer = BitConverter.ToInt64(unitTableBuffer.ToArray(), i);

            if (unitPointer == 0) continue;

            byte[] unitDataStructure;
            
            try
            {
                unitDataStructure = GameMemoryService.GetMemorySpan(this, (IntPtr)unitPointer, 144).ToArray();
            }
            catch ( DataException )
            {
                continue;
            }
            
            var unitType = BitConverter.ToUInt32(unitDataStructure);

            // TODO: Implement unit type enum. - Comment by M9 on 07/18/2024 @ 15:30:04
            // 0 = Player
            if ( unitType == 0 )
            {
                var playerDataPointer = BitConverter.ToInt64(unitDataStructure, 0x10);
                var playerDataStructure        = GameMemoryService.GetMemorySpan(this, (IntPtr)playerDataPointer, 144);

                var playerName = string.Empty;
                
                for (var charIndex = 0; charIndex < 16; charIndex++)
                {
                    if ( playerDataStructure[charIndex] != 0x00 )
                    {
                        playerName += (char)playerDataStructure[charIndex];
                    }
                }

                var playerPathPointer = BitConverter.ToInt64(unitDataStructure, 0x38);
                var playerPathStructure    = GameMemoryService.GetMemorySpan(this, (IntPtr)playerPathPointer, 144);
                
                if ( BitConverter.ToInt16(playerPathStructure.ToArray(), 2) != 0 && 
                     playerName.Equals("Testo_Two", StringComparison.InvariantCultureIgnoreCase) )
                {
                    Pointers.PlayerBaseAddress     = baseUnitAddress;
                    Pointers.PlayerUnitAddress     = (IntPtr)unitPointer;
                    Pointers.PlayerUnitDataAddress = (IntPtr)playerDataPointer;
                    Pointers.PlayerPathAddress     = (IntPtr)playerPathPointer;
                    return;
                }
            }
        }

        throw new DataException("Failed to find player pointers");
    }
} 