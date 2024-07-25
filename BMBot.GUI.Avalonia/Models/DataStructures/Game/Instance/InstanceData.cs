using System;
using System.Collections.Generic;
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
        Game  = new GameData();
        
        ProcessHandle = ProcessInterop.OpenProcess((int)ProcessAccess.PROCESS_QUERY_INFORMATION | (int)ProcessAccess.PROCESS_VM_READ, false, p_applicationProcess.Id);
        
        Pointers.BaseAddress = p_applicationProcess.MainModule.BaseAddress;

        Player = new BarbarianCharacter("Testo_Two");
        
        var refreshTimer = Observable.Interval(TimeSpan.FromSeconds(0.1));
        
        refreshTimer.Subscribe(_ =>
                               {
                                   if ( !Pointers.PointersAreInitialized )
                                   {
                                       return;
                                   }

                                   Game.GameSessionIsActive = GameMemoryService.ReadByte(this, Pointers.GameSessionIsActiveAddress) == 0x1;
                                   
                                   if ( !Game.GameSessionIsActive )
                                   {
                                       if ( !Pointers.PlayerPointersAreInitialized ) return;
                                       
                                       ResetData();

                                       return;
                                   }

                                   if ( !Pointers.PlayerPointersAreInitialized )
                                   {
                                       Thread.Sleep(2500);
                                       FillPlayerPointers();
                                       Player.Id = $"{GameMemoryService.ReadInt32(this, Pointers.PlayerIdAddress):x8}".ToUpper();
                                   }
                                   
                                   UpdateValues();
                               });
    }

    private void ResetData()
    {
        Pointers.PlayerBaseAddress     = IntPtr.Zero;
        Pointers.PlayerUnitAddress     = IntPtr.Zero;
        Pointers.PlayerUnitDataAddress = IntPtr.Zero;
        Pointers.PlayerPathAddress     = IntPtr.Zero;

        Player.XPosition = 0;
        Player.YPosition = 0;
    }

    private void UpdateValues()
    {
        var sw = Stopwatch.StartNew();
        
        UpdateGameValues();

        UpdatePlayerValues();
        
        sw.Stop();

        MemReadTime = $"{sw.ElapsedMilliseconds}ms";

    }

    private void UpdatePlayerValues()
    {
        Player.XPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionXAddress);
        Player.YPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerPositionYAddress);
    }

    private void UpdateGameValues()
    {
        Game.MercIsActive = GameMemoryService.ReadByte(this, Pointers.MercIsActiveAddress) == 0x1;
        
        Game.MiniMapIsEnabled = GameMemoryService.ReadByte(this, Pointers.MiniMapIsEnabledAddress) == 0x1;
        Game.GameMenuIsOpen   = GameMemoryService.ReadByte(this, Pointers.GameMenuIsOpenAddress) == 0x1;
        Game.HelpScreenIsOpen = GameMemoryService.ReadByte(this, Pointers.HelpScreenIsOpenAddress) == 0x1;
        Game.ChatPanelIsOpen  = GameMemoryService.ReadByte(this, Pointers.ChatPanelIsOpenAddress) == 0x1;
        Game.NpcDialogIsActive = GameMemoryService.ReadByte(this, Pointers.NpcDialogIsActiveAddress) == 0x1;
        Game.PortraitsAreEnabled = GameMemoryService.ReadByte(this, Pointers.PortraitsAreEnabledAddress) == 0x1;
        Game.SkillSelectorIsOpen = GameMemoryService.ReadByte(this, Pointers.SkillSelectorIsOpenAddress) == 0x1;
        Game.BeltIsExpanded = GameMemoryService.ReadByte(this, Pointers.BeltIsExpandedAddress) == 0x1;
        
        Game.CharacterScreenIsOpen     = GameMemoryService.ReadByte(this, Pointers.CharacterScreenIsOpenAddress) == 0x1;
        Game.ShopScreenIsOpen          = GameMemoryService.ReadByte(this, Pointers.ShopScreenIsOpenAddress) == 0x1;
        Game.QuestScreenIsOpen         = GameMemoryService.ReadByte(this, Pointers.QuestScreenIsOpenAddress) == 0x1;
        Game.MercInventoryScreenIsOpen = GameMemoryService.ReadByte(this, Pointers.MercInventoryScreenIsOpenAddress) == 0x1;
        Game.ImbueScreenIsOpen         = GameMemoryService.ReadByte(this, Pointers.ImbueScreenIsOpenAddress) == 0x1;
        Game.StashScreenIsOpen         = GameMemoryService.ReadByte(this, Pointers.StashScreenIsOpenAddress) == 0x1;
        Game.CubeScreenIsOpen          = GameMemoryService.ReadByte(this, Pointers.CubeScreenIsOpenAddress) == 0x1;
        Game.PartyScreenIsOpen         = GameMemoryService.ReadByte(this, Pointers.PartyScreenIsOpenAddress) == 0x1;
        Game.WaypointScreenIsOpen      = GameMemoryService.ReadByte(this, Pointers.WaypointScreenIsOpenAddress) == 0x1;
        
        Game.InventoryIsOpen = GameMemoryService.ReadByte(this, Pointers.InventoryIsOpenAddress) == 0x1;
        Game.SkillTreeIsOpen = GameMemoryService.ReadByte(this, Pointers.SkillTreeIsOpenAddress) == 0x1;
    }

    public IntPtr ProcessHandle { get; }
    
    public InstancePointers Pointers { get; } = new();

    [Reactive] public string MemReadTime { get; set; } = string.Empty;
    
    public WindowData           Window     { get; }
    public GameData             Game   { get; }
    public ICharacter           Player  { get; }

    public void GetNearbyNpcs()
    {
        if ( !Pointers.PointersAreInitialized ) return;
        
        const int arraySize = ( 128 + 516 ) * 10;
        
        var unitTableBuffer = GameMemoryService.GetMemorySpan(this, Pointers.UnitTableAddress, arraySize);

        var npcUnits = new List<IntPtr>();
        
        for ( var i = 0; i < unitTableBuffer.Length; i += 8 )
        {
            var baseUnitAddress = Pointers.UnitTableAddress + i;
            var unitPointer     = BitConverter.ToInt64(unitTableBuffer.ToArray(), i);

            if ( unitPointer == 0 ) continue;
            
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

            if ( unitType == 1 )
            {
                npcUnits.Add((IntPtr)unitPointer);
            }
        }

        var test = npcUnits;
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