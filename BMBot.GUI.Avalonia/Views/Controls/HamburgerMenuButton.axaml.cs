using System.Windows.Input;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

using Material.Icons;
using Material.Icons.Avalonia;

namespace BMBot.GUI.Avalonia.Views.Controls;

public partial class HamburgerMenuButton : UserControl
{
    public HamburgerMenuButton()
    {
        InitializeComponent();
        
        IconSize = 32;
        IconColor = Brushes.White;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
        
    public static readonly StyledProperty<string> ButtonTextProperty =
        AvaloniaProperty.Register<HamburgerMenuButton, string>(nameof(ButtonText));

    public string ButtonText
    {
        get => GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
        
    public static readonly StyledProperty<ICommand> ClickCommandProperty =
        AvaloniaProperty.Register<HamburgerMenuButton, ICommand>(nameof(ClickCommand));

    public ICommand ClickCommand
    {
        get => GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }
        
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<HamburgerMenuButton, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
        
    public static readonly StyledProperty<MaterialIconKind> IconProperty =
        AvaloniaProperty.Register<HamburgerMenuButton, MaterialIconKind>(nameof(Icon));

    public MaterialIconKind Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public static readonly StyledProperty<double> IconSizeProperty = AvaloniaProperty.Register<HamburgerMenuButton, double>(
                                                                                         "IconSize");

    public double IconSize
    {
        get => GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public static readonly StyledProperty<IBrush> IconColorProperty = AvaloniaProperty.Register<HamburgerMenuButton, IBrush>(
                                                                                         "IconColor");

    public IBrush IconColor
    {
        get => GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
        
    public static readonly StyledProperty<IBrush> TextColorProperty =
        AvaloniaProperty.Register<HamburgerMenuButton, IBrush>(nameof(TextColor));

    public IBrush TextColor
    {
        get => GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }
}