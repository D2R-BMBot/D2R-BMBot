using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// [assembly: publicsVisibleTo("BMBot.GUI.Avalonia")]
[assembly: DisableRuntimeMarshalling]

namespace BMBot.Interop.API.Window;

public static partial class WindowInterop
{
    [LibraryImport("user32.dll", EntryPoint = "GetClientRect")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetClientRect(int p_windowHandle, out Rectangle p_clientRectangle);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ClientToScreen(int p_windowHandle, out Point p_clientPoint);
}