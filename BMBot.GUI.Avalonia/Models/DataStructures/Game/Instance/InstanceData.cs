using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Threading;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;
using BMBot.GUI.Avalonia.Models.Services.Interop.Memory;
using BMBot.Interop.API.Process;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class InstanceData : ReactiveObject
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

        Character = new BarbarianCharacter("Testo_Two");
        
        var refreshTimer = Observable.Interval(TimeSpan.FromSeconds(0.5));
        
        refreshTimer.Subscribe(_ =>
                               {
                                   if ( !Pointers.PointersAreInitialized )
                                   {
                                       return;
                                   }

                                   Character.CharacterIsInGame = GameMemoryService.ReadByte(this, Pointers.PlayerIsInGameAddress) == 0x1;
                                   
                                   if ( !Character.CharacterIsInGame )
                                   {
                                       if ( !Pointers.PlayerPointersAreInitialized ) return;
                                       
                                       Pointers.PlayerBaseAddress     = IntPtr.Zero;
                                       Pointers.PlayerUnitAddress     = IntPtr.Zero;
                                       Pointers.PlayerUnitDataAddress = IntPtr.Zero;
                                       Pointers.PlayerPathAddress     = IntPtr.Zero;

                                       Character.XPosition = 0;
                                       Character.YPosition = 0;

                                       return;
                                   }

                                   if ( !Pointers.PlayerPointersAreInitialized )
                                   {
                                       Thread.Sleep(2500);
                                       FillPlayerPointers();
                                       Character.CharacterId = $"{GameMemoryService.ReadInt32(this, Pointers.PlayerIdAddress):x8}".ToUpper();
                                   }
                                   
                                   var sw = Stopwatch.StartNew();

                                   Character.XPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionXAddress);
                                   Character.YPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionYAddress);
                                   
                                   sw.Stop();

                                   MemReadTime = $"{sw.ElapsedMilliseconds}ms";
                               });
    }
    
    public IntPtr ProcessHandle { get; }
    
    public InstancePointers Pointers { get; } = new();
    
    [Reactive] public string MemReadTime { get; set; }
    
    public WindowData Window { get; }
    public ICharacter Character  { get; private set; }

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
                     playerName.Equals("Bimbus", StringComparison.InvariantCultureIgnoreCase) )
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