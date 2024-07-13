using System;

using BMBot.GUI.Avalonia.Models.DataStructures.Inventory;
using BMBot.GUI.Avalonia.Models.DataStructures.UI.AccountData;
using BMBot.GUI.Avalonia.Models.Enumerations.Game;

namespace BMBot.GUI.Avalonia.Models.DataStructures.AccountData.Characters;

public class AssassinCharacter(string p_name) : ICharacter
{
    public string  DisplayName   { get; set; } = p_name;

    public bool    CharacterIsBusy { get; set; }
    public Account ParentAccount   { get; }
    
    public InventoryData Inventory { get; } = new();
    
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