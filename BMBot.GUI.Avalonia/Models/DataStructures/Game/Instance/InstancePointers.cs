﻿using System;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class InstancePointers
{
    public IntPtr BaseAddress { get; set; } = 0x0;

    public IntPtr UnitTableAddress => BaseAddress + 0x22DA110 - 0x20;

    #region Player Data

    public IntPtr PlayerPointerAddress           { get; set; } = 0x00;
    public IntPtr PlayerUnitAddress              { get; set; } = 0x00;
    public IntPtr CurrentActPointerAddress       => PlayerUnitAddress + 0x20;
    public IntPtr CurrentActAddress              { get; set; } = 0x00;
    public IntPtr ActMiscellaneousPointerAddress => CurrentActAddress + 0x78;
    public IntPtr ActMiscellaneousAddress        { get; set; } = 0x00;
    public IntPtr SeedHashStartAddress           => ActMiscellaneousAddress + 0x840;
    public IntPtr SeedHashEndAddress             => ActMiscellaneousAddress + 0x868;
    public IntPtr PlayerUnitDataPointerAddress   => PlayerUnitAddress       + 0x10;
    public IntPtr PlayerUnitDataAddress          { get; set; } = 0x00;
    public IntPtr PlayerUnitPathPointerAddress   => PlayerUnitAddress + 0x38;
    public IntPtr PlayerUnitPathAddress          { get; set; } = 0x00;
    public IntPtr PlayerXPositionAddress         => PlayerUnitPathAddress + 0x02;
    public IntPtr PlayerXPositionOffsetAddress   => PlayerUnitPathAddress;
    public IntPtr PlayerYPositionAddress         => PlayerUnitPathAddress + 0x06;
    public IntPtr PlayerYPositionOffsetAddress   => PlayerUnitPathAddress + 0x04;
    public IntPtr PlayerIdAddress                => PlayerUnitAddress     + 0x08;

    #endregion

    public IntPtr GameSessionIsActiveAddress => UnitTableAddress - 0x18;

    #region UI Data

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

    #endregion

    public IntPtr RosterAddress         => BaseAddress + 0x22F0388;
    public IntPtr ExpansionDataAddress  => BaseAddress + 0x21F5E08;
    public IntPtr GameDataAddress       => BaseAddress + 0x29DBCD8;
    public IntPtr MenuVisibilityAddress => BaseAddress + 0x21F92C8;
    public IntPtr LastHoverAddress      => BaseAddress + 0x21F6920;

    public bool PointersAreInitialized       => BaseAddress          != IntPtr.Zero;
    public bool PlayerPointersAreInitialized => PlayerPointerAddress != IntPtr.Zero;
}