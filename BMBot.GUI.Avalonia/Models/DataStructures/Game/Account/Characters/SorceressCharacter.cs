using System;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters.Inventory;
using BMBot.GUI.Avalonia.Models.Enumerations.Game;

using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;

public class SorceressCharacter(string p_name) : ICharacter
{
    public string  DisplayName   { get; set; } = p_name;

    [Reactive] public string      CharacterId       { get; set; } = string.Empty;
    [Reactive] public bool        CharacterIsBusy   { get; set; }
    public            GameAccount ParentGameAccount { get; }
    [Reactive] public short       XPosition         { get; set; }
    [Reactive] public short       YPosition         { get; set; }

    [Reactive] public bool        CharacterIsInGame   { get; set; }
    public            InventoryData Inventory         { get; } = new();
    
    public ConsoleKey LeftAttackKey  { get; set; }
    public ConsoleKey RightAttackKey { get; set; }
    public ConsoleKey InventoryKey   { get; set; }
    public ConsoleKey GearSwapKey    { get; set; }
    
    public ConsoleKey Potion1Key { get; set; }
    public ConsoleKey Potion2Key { get; set; }
    public ConsoleKey Potion3Key { get; set; }
    public ConsoleKey Potion4Key { get; set; }
    
    public PotionType PotionType1 { get; set; }
    public PotionType PotionType2 { get; set; }
    public PotionType PotionType3 { get; set; }
    public PotionType PotionType4 { get; set; }
}