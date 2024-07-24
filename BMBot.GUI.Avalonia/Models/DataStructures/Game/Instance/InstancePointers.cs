using System;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class InstancePointers
{
    public IntPtr BaseAddress { get; set; } = 0x0;

    public IntPtr UnitTableAddress => BaseAddress + 0x22DA110 - 0x20;

    #region Player Data

    public IntPtr PlayerBaseAddress      { get; set; } = 0x00;
    public IntPtr PlayerUnitAddress      { get; set; } = 0x00;
    public IntPtr PlayerUnitDataAddress  { get; set; } = 0x00;
    public IntPtr PlayerPathAddress      { get; set; } = 0x00;
    public IntPtr PlayerPositionXAddress => PlayerPathAddress     + 0x02;
    public IntPtr PlayerPositionYAddress => PlayerPathAddress     + 0x06;
    public IntPtr PlayerIdAddress        => PlayerUnitAddress + 0x08;

    public IntPtr PlayerIsInGameAddress => UnitTableAddress - 0x18;

    #endregion

    public IntPtr RosterAddress               => BaseAddress   + 0x22F0388;
    public IntPtr UiDataAddress               => BaseAddress   + 0x22E9E00;
    public IntPtr CharacterPanelIsOpenAddress => UiDataAddress - 0x06;
    public IntPtr ExpansionDataAddress        => BaseAddress   + 0x21F5E08;
    public IntPtr GameDataAddress             => BaseAddress   + 0x29DBCD8;
    public IntPtr MenuVisibilityAddress       => BaseAddress   + 0x21F92C8;
    public IntPtr LastHoverAddress            => BaseAddress   + 0x21F6920;

    public bool PointersAreInitialized       => BaseAddress       != IntPtr.Zero;
    public bool PlayerPointersAreInitialized => PlayerBaseAddress != IntPtr.Zero;
}