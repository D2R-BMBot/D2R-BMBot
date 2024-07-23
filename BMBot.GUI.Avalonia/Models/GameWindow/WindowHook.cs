using System;
using System.Runtime.InteropServices;

namespace BMBot.GUI.Avalonia.Models.GameWindow;

public class WindowHook
{
    public delegate void WinEventDelegate(
        IntPtr                      p_hWinEventHook,
        NativeMethods.SwehEvents   p_eventType,
        IntPtr                      p_windowHandle,
        NativeMethods.SwehObjectId p_idObject,
        long                        p_idChild,
        uint                        p_dwEventThread,
        uint                        p_dwmsEventTime
    );

    public static IntPtr WinEventHookRange(
        NativeMethods.SwehEvents p_eventFrom,
        NativeMethods.SwehEvents p_eventTo,
        WinEventDelegate          p_eventDelegate,
        uint                      p_idProcess,
        uint                      p_idThread)
    {
        return NativeMethods.SetWinEventHook(
                                             p_eventFrom,
                                             p_eventTo,
                                             IntPtr.Zero,
                                             p_eventDelegate,
                                             p_idProcess, p_idThread,
                                             NativeMethods.WinEventHookInternalFlags);
    }

    public static IntPtr WinEventHookOne(
        NativeMethods.SwehEvents p_eventId,
        WinEventDelegate         p_eventDelegate,
        uint                     p_idProcess,
        uint                     p_idThread)
    {
        return NativeMethods.SetWinEventHook(p_eventId, 
                                             p_eventId,
                                             IntPtr.Zero,
                                             p_eventDelegate,
                                             p_idProcess, 
                                             p_idThread,
                                             NativeMethods.WinEventHookInternalFlags);
    }

    public static bool WinEventUnhook(IntPtr p_hWinEventHook) =>
        NativeMethods.UnhookWinEvent(p_hWinEventHook);

    public static uint GetWindowThread(IntPtr p_hWnd)
    {
        return NativeMethods.GetWindowThreadProcessId(p_hWnd, IntPtr.Zero);
    }

    public static NativeMethods.Rect GetWindowRectangle(IntPtr p_hWnd)
    {
        NativeMethods.DwmGetWindowAttribute(p_hWnd,
                                            NativeMethods.Dwmwindowattribute.DwmwaExtendedFrameBounds,
                                            out NativeMethods.Rect rect, Marshal.SizeOf<NativeMethods.Rect>());
        return rect;
    }
}