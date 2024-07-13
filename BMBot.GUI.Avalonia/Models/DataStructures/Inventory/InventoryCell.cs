using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Inventory;

public class InventoryCell : ReactiveObject
{
    [Reactive] public bool IsOccupied { get; set; }
    // TODO: Implement item list. - Comment by M9 on 07/12/2024 @ 00:00:00
    // [Reactive] public Item Item { get; set; }
}