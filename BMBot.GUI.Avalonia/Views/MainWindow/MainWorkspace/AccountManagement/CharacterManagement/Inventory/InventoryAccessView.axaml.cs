using Avalonia;
using Avalonia.Controls;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Account.Characters.Inventory;

namespace BMBot.GUI.Avalonia.Views.MainWindow.MainWorkspace.AccountManagement.CharacterManagement.Inventory;

public partial class InventoryAccessView : UserControl
{
    public InventoryAccessView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<InventoryData> InventoryProperty = AvaloniaProperty.Register<InventoryAccessView, InventoryData>(
                                                                                         "Inventory");

    public InventoryData Inventory
    {
        get => GetValue(InventoryProperty);
        set => SetValue(InventoryProperty, value);
    }
}