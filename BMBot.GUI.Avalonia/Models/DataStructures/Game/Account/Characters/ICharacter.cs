using System;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters.Inventory;
using BMBot.GUI.Avalonia.Models.Enumerations.Game;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters;

public interface ICharacter : IAccountItem
{
    string      CharacterId       { get; set; }
    bool        CharacterIsBusy   { get; }
    GameAccount ParentGameAccount { get; }
    
    short XPosition { get; set; }
    short YPosition { get; set; }
    
    bool CharacterIsInGame { get; set; }
    
    InventoryData Inventory { get; }

    ConsoleKey LeftAttackKey  { get; }
    ConsoleKey RightAttackKey { get; }
    ConsoleKey InventoryKey   { get; }
    ConsoleKey GearSwapKey    { get; }
    
    ConsoleKey Potion1Key { get; }
    ConsoleKey Potion2Key { get; }
    ConsoleKey Potion3Key { get; }
    ConsoleKey Potion4Key { get; }
    
    PotionType PotionType1 { get; }
    PotionType PotionType2 { get; }
    PotionType PotionType3 { get; }
    PotionType PotionType4 { get; }
    
    
}