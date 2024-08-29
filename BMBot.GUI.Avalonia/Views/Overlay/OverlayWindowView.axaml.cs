using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Avalonia;
using Avalonia.Controls;

namespace BMBot.GUI.Avalonia.Views.Overlay;

public partial class OverlayWindowView : Window
{
    [DllImport("user32.dll")]
    private static extern void SetWindowLong(IntPtr p_hWnd, int p_nIndex, uint p_dwNewLong);
    
    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr p_hWnd, int p_nIndex);
    
    private const int  GwlExStyle      = -20;
    private const uint WsExTransparent = 0x00000020;
    private const uint WsExLayered     = 0x00080000;

    public OverlayWindowView()
    {
        InitializeComponent();

        var handle = TryGetPlatformHandle()?.Handle ?? 0x00;
        
        var initialStyle = GetWindowLong(handle, GwlExStyle);
        
        SetWindowLong(handle, GwlExStyle, initialStyle | WsExTransparent | WsExLayered);
    }
    
    public void SetPosition(int p_xPosition, int p_yPosition)
    {
        Position = new PixelPoint(p_xPosition, p_yPosition);
    }
}