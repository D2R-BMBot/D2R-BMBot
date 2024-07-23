using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using BMBot.GUI.Avalonia.Models.GameWindow;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class WindowData : IDisposable
{
    [DllImport("user32.dll")]
    private static extern void SetWindowLong(IntPtr p_hWnd, int p_nIndex, uint p_dwNewLong);

    [DllImport("user32.dll")]
    private static extern uint GetWindowLong(IntPtr p_hWnd, int p_nIndex);

    private const int  GwlExStyle      = -20;
    private const uint WsExTransparent = 0x00000020;
    private const uint WsExLayered     = 0x00080000;

    private readonly IntPtr m_windowHandle;

    private readonly IntPtr m_windowChangedHook;
    private readonly IntPtr m_windowClosedHook;
    private readonly IntPtr m_windowForegroundHook;

    private static GCHandle m_gcSafetyHandle;

    internal WindowData(Process p_process)
    {
        m_windowHandle = p_process.MainWindowHandle;

        WindowHook.WinEventDelegate winEventDelegate = WinEventCallback;
        m_gcSafetyHandle = GCHandle.Alloc(winEventDelegate);
    }

    public bool Topmost   { get; private set; }
    public bool IsVisible { get; private set; }
    public int  XPosition { get; private set; }
    public int  YPosition { get; private set; }
    public int  Width     { get; private set; }
    public int  Height    { get; private set; }

    public float WindowCenterX => XPosition + Width  / 2.0f;
    public float WindowCenterY => YPosition + Height / 2.0f;
    
    public Action CloseAction { get; set; }

    private void WinEventCallback(
        IntPtr                     p_hWinEventHook,
        NativeMethods.SwehEvents   p_eventType,
        IntPtr                     p_hWnd,
        NativeMethods.SwehObjectId p_idObject,
        long                       p_idChild,
        uint                       p_dwEventThread,
        uint                       p_dwmsEventTime)
    {
        if ( p_hWnd      != m_windowHandle                                 &&
             p_eventType == NativeMethods.SwehEvents.EventSystemForeground &&
             p_idObject  == (NativeMethods.SwehObjectId)NativeMethods.SwehChildIdSelf )
        {
            Topmost   = false;
            IsVisible = false;
            return;
        }

        if ( p_eventType != NativeMethods.SwehEvents.EventObjectDestroy        &&
             p_eventType != NativeMethods.SwehEvents.EventObjectLocationchange &&
             p_eventType != NativeMethods.SwehEvents.EventSystemForeground     &&
             p_eventType != NativeMethods.SwehEvents.EventObjectFocus ||
             p_idObject != (NativeMethods.SwehObjectId)NativeMethods.SwehChildIdSelf )
        {
            return;
        }

        switch ( p_eventType )
        {
            case NativeMethods.SwehEvents.EventObjectLocationchange:
                var rect = WindowHook.GetWindowRectangle(p_hWnd);

                XPosition = rect.Left;
                YPosition = rect.Top;
                Width     = rect.Right  - rect.Left;
                Height    = rect.Bottom - rect.Top;
                break;
            case NativeMethods.SwehEvents.EventObjectDestroy:
                IsVisible = false;
                CloseAction();
                break;
            case NativeMethods.SwehEvents.EventSystemForeground:
                Topmost   = true;
                IsVisible = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(p_eventType));
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}