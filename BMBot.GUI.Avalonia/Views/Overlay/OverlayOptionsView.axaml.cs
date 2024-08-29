using System;
using System.Runtime.InteropServices;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace BMBot.GUI.Avalonia.Views.Overlay;

public partial class OverlayOptionsView : Window
{
    [DllImport("user32.dll")]
    private static extern void SetWindowLong(IntPtr p_hWnd, int p_nIndex, uint p_dwNewLong);
    
    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr p_hWnd, int p_nIndex);
    
    private const int  GwlExStyle      = -20;
    private const uint WsExTransparent = 0x00000020;
    private const uint WsExLayered     = 0x00080000;
    
    public OverlayOptionsView()
    {
        InitializeComponent();
    }
    
    public void SetPosition(int p_xPosition, int p_yPosition)
    {
        Position = new PixelPoint(p_xPosition + 58, p_yPosition + 52);
    }
}