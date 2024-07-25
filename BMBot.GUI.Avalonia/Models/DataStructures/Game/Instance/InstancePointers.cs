using System;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class InstancePointers : ReactiveObject
{
    [Reactive] public IntPtr BaseAddress { get; set; } = 0x0;

    public IntPtr UnitTableAddress => BaseAddress + 0x22DA110 - 0x20;

    #region Player Data

    [Reactive] public IntPtr PlayerBaseAddress      { get; set; } = 0x00;
    [Reactive] public IntPtr PlayerUnitAddress      { get; set; } = 0x00;
    [Reactive] public IntPtr PlayerUnitDataAddress  { get; set; } = 0x00;
    [Reactive] public IntPtr PlayerPathAddress      { get; set; } = 0x00;
    public            IntPtr PlayerPositionXAddress => PlayerPathAddress + 0x02;
    public            IntPtr PlayerPositionYAddress => PlayerPathAddress + 0x06;
    public            IntPtr PlayerIdAddress        => PlayerUnitAddress + 0x08;

    #endregion

    public IntPtr GameSessionIsActiveAddress => UnitTableAddress - 0x18;

    public IntPtr UiDataAddress                    => BaseAddress   + 0x22E9E00;
    public IntPtr MercIsActiveAddress              => UiDataAddress + 0x0A;
    public IntPtr MiniMapIsEnabledAddress          => UiDataAddress + 0x02;
    public IntPtr GameMenuIsOpenAddress            => UiDataAddress + 0x01;
    public IntPtr HelpScreenIsOpenAddress          => UiDataAddress + 0x13;
    public IntPtr ChatPanelIsOpenAddress           => UiDataAddress - 0x03;
    public IntPtr NpcDialogIsActiveAddress         => UiDataAddress;
    public IntPtr PortraitsAreEnabledAddress       => UiDataAddress + 0x15;
    public IntPtr SkillSelectorIsOpenAddress       => UiDataAddress - 0x05;
    public IntPtr BeltIsExpandedAddress            => UiDataAddress + 0x12;
    public IntPtr CharacterScreenIsOpenAddress     => UiDataAddress - 0x06;
    public IntPtr ShopScreenIsOpenAddress          => UiDataAddress + 0x03;
    public IntPtr QuestScreenIsOpenAddress         => UiDataAddress + 0x06;
    public IntPtr MercInventoryScreenIsOpenAddress => UiDataAddress + 0x16;
    public IntPtr ImbueScreenIsOpenAddress         => UiDataAddress + 0x05;
    public IntPtr StashScreenIsOpenAddress         => UiDataAddress + 0x10;
    public IntPtr CubeScreenIsOpenAddress          => UiDataAddress + 0x11;
    public IntPtr PartyScreenIsOpenAddress         => UiDataAddress + 0x0D;
    public IntPtr WaypointScreenIsOpenAddress      => UiDataAddress + 0x0B;
    public IntPtr InventoryIsOpenAddress           => UiDataAddress - 0x07;
    public IntPtr SkillTreeIsOpenAddress           => UiDataAddress - 0x04;
    public IntPtr RosterAddress                    => BaseAddress   + 0x22F0388;
    public IntPtr ExpansionDataAddress             => BaseAddress   + 0x21F5E08;
    public IntPtr GameDataAddress                  => BaseAddress   + 0x29DBCD8;
    public IntPtr MenuVisibilityAddress            => BaseAddress   + 0x21F92C8;
    public IntPtr LastHoverAddress                 => BaseAddress   + 0x21F6920;

    public bool PointersAreInitialized       => BaseAddress       != IntPtr.Zero;
    public bool PlayerPointersAreInitialized => PlayerBaseAddress != IntPtr.Zero;
}