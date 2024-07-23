using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

using Avalonia;
using Avalonia.Controls;

using BMBot.GUI.Avalonia.Models.GameWindow;

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

    private readonly IntPtr   d2WindowHandle;
    private readonly IntPtr   windowChangedHook;
    private readonly IntPtr   windowClosedHook;
    private readonly IntPtr   windowForegroundHook;
    private readonly Process? targetProcess;

    private static   GCHandle                    m_gcSafetyHandle;

    public OverlayWindowView()
    {
        InitializeComponent();

        var handle = TryGetPlatformHandle()?.Handle ?? 0x00;

        var initialStyle = GetWindowLong(handle, GwlExStyle);

        SetWindowLong(handle, GwlExStyle, initialStyle | WsExTransparent | WsExLayered);

        WindowHook.WinEventDelegate winEventDelegate = WinEventCallback;
        m_gcSafetyHandle = GCHandle.Alloc(winEventDelegate);

        targetProcess = Process.GetProcessesByName("D2R").FirstOrDefault();

        if ( targetProcess is null ) return;

        d2WindowHandle = targetProcess.MainWindowHandle;

        if ( d2WindowHandle == IntPtr.Zero ) return;

        var targetThreadId = WindowHook.GetWindowThread(d2WindowHandle);

        windowChangedHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventObjectLocationchange,
                                                       winEventDelegate, (uint)targetProcess.Id, targetThreadId);

        windowClosedHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventObjectDestroy,
                                                      winEventDelegate, (uint)targetProcess.Id, targetThreadId);
        
        windowForegroundHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventSystemForeground,
                                                      winEventDelegate, 0, 0);

        var rect = WindowHook.GetWindowRectangle(d2WindowHandle);

        Position = new PixelPoint(rect.Left, rect.Top);
        Width    = rect.Right  - rect.Left;
        Height   = rect.Bottom - rect.Top;
    }

    private void WinEventCallback(
        IntPtr                     p_hWinEventHook,
        NativeMethods.SwehEvents   p_eventType,
        IntPtr                     p_hWnd,
        NativeMethods.SwehObjectId p_idObject,
        long                       p_idChild,
        uint                       p_dwEventThread,
        uint                       p_dwmsEventTime)
    {
        if ( p_hWnd      != d2WindowHandle                                 &&
             p_eventType == NativeMethods.SwehEvents.EventSystemForeground &&
             p_idObject  == (NativeMethods.SwehObjectId)NativeMethods.SwehChildIdSelf )
        {
            Topmost = false;
            Hide();
            return;
        }

        if ( p_eventType != NativeMethods.SwehEvents.EventObjectDestroy        &&
             p_eventType != NativeMethods.SwehEvents.EventObjectLocationchange &&
             p_eventType != NativeMethods.SwehEvents.EventSystemForeground     &&
             p_eventType != NativeMethods.SwehEvents.EventObjectFocus          ||
             p_idObject != (NativeMethods.SwehObjectId)NativeMethods.SwehChildIdSelf )
        {
            return;
        }

        switch ( p_eventType )
        {
            case NativeMethods.SwehEvents.EventObjectLocationchange:
                var rect = WindowHook.GetWindowRectangle(p_hWnd);

                Position = new PixelPoint(rect.Left, rect.Top);
                Width    = rect.Right  - rect.Left;
                Height   = rect.Bottom - rect.Top;
                break;
            case NativeMethods.SwehEvents.EventObjectDestroy:
                Hide();
                Close();
                break;
            case NativeMethods.SwehEvents.EventSystemForeground:
                Topmost = true;
                Show();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(p_eventType));
        }
    }

    protected override void OnClosing(WindowClosingEventArgs p_e)
    {
        base.OnClosing(p_e);
        if ( !p_e.Cancel )
        {
            if ( m_gcSafetyHandle.IsAllocated ) m_gcSafetyHandle.Free();
            WindowHook.WinEventUnhook(windowChangedHook);
            WindowHook.WinEventUnhook(windowClosedHook);
            WindowHook.WinEventUnhook(windowForegroundHook);
        }
    }

    protected override void OnOpened(EventArgs p_e)
    {
        if ( targetProcess == null )
        {
            Hide();
            Close();
        }
        else
        {
            targetProcess.Dispose();
        }

        base.OnOpened(p_e);
    }
}