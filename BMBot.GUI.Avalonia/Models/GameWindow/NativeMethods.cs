using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BMBot.GUI.Avalonia.Models.GameWindow;

public static partial class NativeMethods
{
    [LibraryImport("dwmapi.dll", SetLastError = true)]
    public static partial int DwmGetWindowAttribute(IntPtr p_hwnd, Dwmwindowattribute p_dwAttribute, out Rect p_pvAttribute, int p_cbAttribute);

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);
    }

    public static long SwehChildIdSelf = 0;

    //SetWinEventHook() flags
    public enum SwehDwFlags : uint
    {
        WineventOutofcontext = 0x0000,     // Events are ASYNC
        WineventSkipownthread = 0x0001,    // Don't call back for events on installer's thread
        WineventSkipownprocess = 0x0002,   // Don't call back for events on installer's process
        WineventIncontext = 0x0004         // Events are SYNC, this causes your dll to be injected into every process
    }

    //SetWinEventHook() events
    [Flags]
    public enum SwehEvents : uint
    {
        EventMin = 0x00000001,
        EventMax = 0x7FFFFFFF,
        EventSystemSound = 0x0001,
        EventSystemAlert = 0x0002,
        EventSystemForeground = 0x0003,
        EventSystemMenustart = 0x0004,
        EventSystemMenuend = 0x0005,
        EventSystemMenupopupstart = 0x0006,
        EventSystemMenupopupend = 0x0007,
        EventSystemCapturestart = 0x0008,
        EventSystemCaptureend = 0x0009,
        EventSystemMovesizestart = 0x000A,
        EventSystemMovesizeend = 0x000B,
        EventSystemContexthelpstart = 0x000C,
        EventSystemContexthelpend = 0x000D,
        EventSystemDragdropstart = 0x000E,
        EventSystemDragdropend = 0x000F,
        EventSystemDialogstart = 0x0010,
        EventSystemDialogend = 0x0011,
        EventSystemScrollingstart = 0x0012,
        EventSystemScrollingend = 0x0013,
        EventSystemSwitchstart = 0x0014,
        EventSystemSwitchend = 0x0015,
        EventSystemMinimizestart = 0x0016,
        EventSystemMinimizeend = 0x0017,
        EventSystemDesktopswitch = 0x0020,
        EventSystemEnd = 0x00FF,
        EventOemDefinedStart = 0x0101,
        EventOemDefinedEnd = 0x01FF,
        EventUiaEventidStart = 0x4E00,
        EventUiaEventidEnd = 0x4EFF,
        EventUiaPropidStart = 0x7500,
        EventUiaPropidEnd = 0x75FF,
        EventConsoleCaret = 0x4001,
        EventConsoleUpdateRegion = 0x4002,
        EventConsoleUpdateSimple = 0x4003,
        EventConsoleUpdateScroll = 0x4004,
        EventConsoleLayout = 0x4005,
        EventConsoleStartApplication = 0x4006,
        EventConsoleEndApplication = 0x4007,
        EventConsoleEnd = 0x40FF,
        EventObjectCreate = 0x8000,               // hwnd ID idChild is created item
        EventObjectDestroy = 0x8001,              // hwnd ID idChild is destroyed item
        EventObjectShow = 0x8002,                 // hwnd ID idChild is shown item
        EventObjectHide = 0x8003,                 // hwnd ID idChild is hidden item
        EventObjectReorder = 0x8004,              // hwnd ID idChild is parent of zordering children
        EventObjectFocus = 0x8005,                // hwnd ID idChild is focused item
        EventObjectSelection = 0x8006,            // hwnd ID idChild is selected item (if only one), or idChild is OBJID_WINDOW if complex
        EventObjectSelectionadd = 0x8007,         // hwnd ID idChild is item added
        EventObjectSelectionremove = 0x8008,      // hwnd ID idChild is item removed
        EventObjectSelectionwithin = 0x8009,      // hwnd ID idChild is parent of changed selected items
        EventObjectStatechange = 0x800A,          // hwnd ID idChild is item w/ state change
        EventObjectLocationchange = 0x800B,       // hwnd ID idChild is moved/sized item
        EventObjectNamechange = 0x800C,           // hwnd ID idChild is item w/ name change
        EventObjectDescriptionchange = 0x800D,    // hwnd ID idChild is item w/ desc change
        EventObjectValuechange = 0x800E,          // hwnd ID idChild is item w/ value change
        EventObjectParentchange = 0x800F,         // hwnd ID idChild is item w/ new parent
        EventObjectHelpchange = 0x8010,           // hwnd ID idChild is item w/ help change
        EventObjectDefactionchange = 0x8011,      // hwnd ID idChild is item w/ def action change
        EventObjectAcceleratorchange = 0x8012,    // hwnd ID idChild is item w/ keybd accel change
        EventObjectInvoked = 0x8013,              // hwnd ID idChild is item invoked
        EventObjectTextselectionchanged = 0x8014, // hwnd ID idChild is item w? test selection change
        EventObjectContentscrolled = 0x8015,
        EventSystemArrangmentpreview = 0x8016,
        EventObjectEnd = 0x80FF,
        EventAiaStart = 0xA000,
        EventAiaEnd = 0xAFFF
    }

    //SetWinEventHook() Object Ids
    public enum SwehObjectId : long
    {
        ObjidWindow = 0x00000000,
        ObjidSysmenu = 0xFFFFFFFF,
        ObjidTitlebar = 0xFFFFFFFE,
        ObjidMenu = 0xFFFFFFFD,
        ObjidClient = 0xFFFFFFFC,
        ObjidVscroll = 0xFFFFFFFB,
        ObjidHscroll = 0xFFFFFFFA,
        ObjidSizegrip = 0xFFFFFFF9,
        ObjidCaret = 0xFFFFFFF8,
        ObjidCursor = 0xFFFFFFF7,
        ObjidAlert = 0xFFFFFFF6,
        ObjidSound = 0xFFFFFFF5,
        ObjidQueryclassnameidx = 0xFFFFFFF4,
        ObjidNativeom = 0xFFFFFFF0
    }

    public enum Dwmwindowattribute : uint
    {
        DwmwaNcrenderingEnabled = 1,      // [get] Is non-client rendering enabled/disabled
        DwmwaNcrenderingPolicy,           // [set] DWMNCRENDERINGPOLICY - Non-client rendering policy - Enable or disable non-client rendering
        DwmwaTransitionsForcedisabled,    // [set] Potentially enable/forcibly disable transitions
        DwmwaAllowNcpaint,                // [set] Allow contents rendered In the non-client area To be visible On the DWM-drawn frame.
        DwmwaCaptionButtonBounds,        // [get] Bounds Of the caption button area In window-relative space.
        DwmwaNonclientRtlLayout,         // [set] Is non-client content RTL mirrored
        DwmwaForceIconicRepresentation,  // [set] Force this window To display iconic thumbnails.
        DwmwaFlip3DPolicy,                // [set] Designates how Flip3D will treat the window.
        DwmwaExtendedFrameBounds,        // [get] Gets the extended frame bounds rectangle In screen space
        DwmwaHasIconicBitmap,            // [set] Indicates an available bitmap When there Is no better thumbnail representation.
        DwmwaDisallowPeek,                // [set] Don't invoke Peek on the window.
        DwmwaExcludedFromPeek,           // [set] LivePreview exclusion information
        DwmwaCloak,                        // [set] Cloak Or uncloak the window
        DwmwaCloaked,                      // [get] Gets the cloaked state Of the window. Returns a DWMCLOACKEDREASON object
        DwmwaFreezeRepresentation,        // [set] BOOL, Force this window To freeze the thumbnail without live update
        PlaceHolder1,
        PlaceHolder2,
        PlaceHolder3,
        DwmwaAccentpolicy = 19
    }

    public static SwehDwFlags WinEventHookInternalFlags =
        SwehDwFlags.WineventOutofcontext |
        SwehDwFlags.WineventSkipownprocess |
        SwehDwFlags.WineventSkipownthread;

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr p_hWnd, out uint p_lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr p_hWnd, IntPtr p_voidProcessId);

    [DllImport("user32.dll", SetLastError = false)]
    public static extern IntPtr SetWinEventHook(
        SwehEvents                             p_eventMin,
        SwehEvents                             p_eventMax,
        IntPtr                                 p_hmodWinEventProc,
        GameWindow.WindowHook.WinEventDelegate p_lpfnWinEventProc,
        uint                                   p_idProcess, uint p_idThread,
        SwehDwFlags                            p_dwFlags);

    [DllImport("user32.dll", SetLastError = false)]
    public static extern bool UnhookWinEvent(IntPtr p_hWinEventHook);

}