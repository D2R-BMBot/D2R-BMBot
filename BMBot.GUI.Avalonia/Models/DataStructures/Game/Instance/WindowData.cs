using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Avalonia;

using BMBot.GUI.Avalonia.Models.GameWindow;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;

public class WindowData : ReactiveObject, IDisposable
{
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

        var targetThreadId = WindowHook.GetWindowThread(m_windowHandle);

        m_windowChangedHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventObjectLocationchange,
                                                         winEventDelegate, (uint)p_process.Id, targetThreadId);

        m_windowClosedHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventObjectDestroy,
                                                        winEventDelegate, (uint)p_process.Id, targetThreadId);

        m_windowForegroundHook = WindowHook.WinEventHookOne(NativeMethods.SwehEvents.EventSystemForeground,
                                                            winEventDelegate, 0, 0);

        this.WhenAnyValue(p_data => p_data.XPosition, p_data => p_data.YPosition)
            .Subscribe(p_data => OnWindowPositionChanged(p_data.Item1, p_data.Item2));
        
        var rect = WindowHook.GetWindowRectangle(m_windowHandle);
        
        XPosition = rect.Left;
        YPosition = rect.Top;
        Width     = rect.Right  - rect.Left;
        Height    = rect.Bottom - rect.Top;
    }

    
    [Reactive] public bool       Topmost   { get; private set; }
    [Reactive] public bool       IsVisible { get; set; }
    [Reactive] public int        XPosition { get; private set; }
    [Reactive] public int        YPosition { get; private set; }
    [Reactive] public int        Width     { get; private set; }
    [Reactive] public int        Height    { get; private set; }

    public float WindowCenterX => XPosition + Width  / 2.0f;
    public float WindowCenterY => YPosition + Height / 2.0f;

    public Action<int,int>? SetWindowPositionAction { get; set; }
    public Action? CloseAction { get; set; }
    
    // public bool 

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
                CloseAction!();
                break;
            case NativeMethods.SwehEvents.EventSystemForeground:
                Topmost   = true;
                IsVisible = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(p_eventType));
        }
    }

    public void RefreshWindowPosition()
    {
        SetWindowPositionAction?.Invoke(XPosition, YPosition);
    }

    private void OnWindowPositionChanged(int p_x, int p_y)
    {
        SetWindowPositionAction?.Invoke(p_x, p_y);
    }
    
    public void Dispose()
    {
        if ( m_gcSafetyHandle.IsAllocated ) m_gcSafetyHandle.Free();
        WindowHook.WinEventUnhook(m_windowChangedHook);
        WindowHook.WinEventUnhook(m_windowClosedHook);
        WindowHook.WinEventUnhook(m_windowForegroundHook);
    }
}