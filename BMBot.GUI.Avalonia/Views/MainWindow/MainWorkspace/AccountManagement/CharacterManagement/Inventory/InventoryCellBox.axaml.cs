using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace BMBot.GUI.Avalonia.Views.MainWindow.MainWorkspace.AccountManagement.CharacterManagement.Inventory;

public partial class InventoryCellBox : UserControl
{
    public InventoryCellBox()
    {
        InitializeComponent();
        
        // IconSize = 32;
    }

    public static readonly StyledProperty<bool> IsOccupiedProperty = AvaloniaProperty.Register<InventoryCellBox, bool>(
                                                                                         "IsOccupied");
    public bool IsOccupied
    {
        get => GetValue(IsOccupiedProperty);
        set => SetValue(IsOccupiedProperty, value);
    }

    public static readonly StyledProperty<double> IconSizeProperty = AvaloniaProperty.Register<InventoryCellBox, double>(
                                                                                         "IconSize");
    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    private void ToggleButton_OnClick(object? p_sender, RoutedEventArgs p_e)
    {
        IsOccupied = !IsOccupied;
    }
}