using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

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
        
        var cursorImage  = AssetLoader.Open(new Uri("avares://BMBot.GUI.Avalonia/Assets/d2Hand.png"));
        var cursorBitmap = new Bitmap(cursorImage);
        
        CustomCursor = new Cursor(cursorBitmap.CreateScaledBitmap(new PixelSize(48, 48), BitmapInterpolationMode.None), new PixelPoint(0, 0));

        Window    = new WindowData(p_applicationProcess);
        Game  = new GameData();

        // Cannot use standard process handle from p_applicationProcess here.
        // Must re-open with specified access parameters. - Comment by M9 on 07/25/2024 @ 00:00:00
        ProcessHandle = ProcessInterop.OpenProcess((int)ProcessAccess.PROCESS_QUERY_INFORMATION | (int)ProcessAccess.PROCESS_VM_READ, false, p_applicationProcess.Id);
        
        Pointers.BaseAddress = p_applicationProcess.MainModule.BaseAddress;

        Player = new BarbarianCharacter("Testo_Two");
        
        var refreshTimer = Observable.Interval(TimeSpan.FromSeconds(0.1))
                                     .SelectMany(_ => Observable.FromAsync(Update));
        
        refreshTimer.Subscribe();
    }

    private async Task Update()
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
            // We can't go anything further until the roster is filled. - Comment by M9 on 07/31/2024 @ 00:00:00
            var rosterPointer = (IntPtr)GameMemoryService.ReadInt64(this, Pointers.RosterAddress);

            if ( rosterPointer == 0x00 )
            {
                // Roster isn't finished initializing. Try again next update. - Comment by M9 on 07/31/2024 @ 00:00:00
                return;
            }
            
            var playerId = GameMemoryService.ReadInt32(this, rosterPointer + 0x48);
            
            FindPlayerAddresses(playerId);
            
            Player.Id = $"{playerId:x8}".ToUpper();

            GetGameSeed();
        }
                                   
        UpdateValues();

        await Task.CompletedTask;
    }

    private void GetGameSeed()
    {
        var seedHashStart = GameMemoryService.ReadUInt32(this, Pointers.SeedHashStartAddress);
        var seedHashEnd   = GameMemoryService.ReadUInt32(this, Pointers.SeedHashEndAddress);

        var seedXor = 0u;
        
        var incrementalValue = 1u;

        var startValue = 0u;
        var valueFound = false;

        for (; startValue < uint.MaxValue; startValue += incrementalValue)
        {
            var seedResult = (startValue * 0x6AC690C5 + 666) & 0xFFFFFFFF;

            if (seedResult == seedHashEnd)
            {
                valueFound = true;
                break;
            }

            if (incrementalValue == 1 && (seedResult % 65536) == (seedHashEnd % 65536))
            {
                incrementalValue = 65536;
            }
        }

        if ( valueFound )
        {
            seedXor = seedHashStart ^ startValue;
        }

        if ( seedXor == 0 )
        {
            throw new DataException("Failed to find game seed hash");
        }
        
        Game.Seed = startValue;
    }

    private void ResetData()
    {
        Pointers.PlayerPointerAddress     = IntPtr.Zero;
        Pointers.PlayerUnitAddress     = IntPtr.Zero;
        Pointers.PlayerUnitDataAddress = IntPtr.Zero;
        Pointers.PlayerUnitPathAddress = IntPtr.Zero;
        Pointers.CurrentActAddress     = IntPtr.Zero;
        Pointers.ActMiscellaneousAddress = IntPtr.Zero;

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
        Player.XPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerXPositionAddress);
        Player.YPosition = GameMemoryService.ReadInt16(this, Pointers.PlayerYPositionAddress);
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

    [Reactive] public RelativePoint TransformOrigin { get; set; }
    [Reactive] public double        RotAngle        { get; set; } = 63.5;
    [Reactive] public double        SkewAngleX      { get; set; }
    [Reactive] public double        SkewAngleY      { get; set; } = -37;
    [Reactive] public double        ScaleX          { get; set; } = 1;
    [Reactive] public double        ScaleY          { get; set; } = 1.3333333;
    public            IntPtr        ProcessHandle   { get; }
    
    public InstancePointers Pointers { get; } = new();

    [Reactive] public string     MemReadTime  { get; set; } = string.Empty;
    [Reactive] public Cursor?    CustomCursor { get; set; }
    public            WindowData Window       { get; }
    public            GameData   Game         { get; }
    public            ICharacter Player       { get; }

    public void GetNearbyNpcs()
    {
        if ( !Pointers.PointersAreInitialized ) return;
        
        const int arraySize = ( 128 + 516 ) * 10;
        
        var unitTableBuffer = GameMemoryService.GetMemorySpan(this, Pointers.UnitTableAddress, arraySize);

        var npcUnits = new List<IntPtr>();
        
        for ( var i = 0; i < unitTableBuffer.Length; i += 8 )
        {
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

    private void FindPlayerAddresses(IntPtr p_playerId)
    {
        if ( !Pointers.PointersAreInitialized ) return;
        
        const int arraySize = ( 128 + 516 ) * 8;

        var unitTableBuffer = GameMemoryService.GetMemorySpan(this, Pointers.UnitTableAddress, arraySize);

        for ( var i = 0; i < unitTableBuffer.Length; i += 8 )
        {
            var baseUnitAddress = Pointers.UnitTableAddress + i;
            var unitPointer = BitConverter.ToInt64(unitTableBuffer.ToArray(), i);

            if (unitPointer == 0) continue;

            IntPtr unitId;

            try 
            {            
                unitId = GameMemoryService.ReadInt32(this, (IntPtr)unitPointer + 0x08);
            }
            catch ( DataException )
            {
                continue;
            }
            
            if ( unitId != p_playerId ) continue;

            Pointers.PlayerPointerAddress  = baseUnitAddress;
            Pointers.PlayerUnitAddress     = (IntPtr)unitPointer;
            Pointers.PlayerUnitDataAddress = (IntPtr)GameMemoryService.ReadInt64(this, Pointers.PlayerUnitDataPointerAddress);
            Pointers.PlayerUnitPathAddress = (IntPtr)GameMemoryService.ReadInt64(this, Pointers.PlayerUnitPathPointerAddress);
            Pointers.CurrentActAddress     = (IntPtr)GameMemoryService.ReadInt64(this, Pointers.CurrentActPointerAddress);
            Pointers.ActMiscellaneousAddress = (IntPtr)GameMemoryService.ReadInt64(this, Pointers.ActMiscellaneousPointerAddress);
            
            return;
        }

        throw new DataException("Failed to find player pointers");
    }
} 