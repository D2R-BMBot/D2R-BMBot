using System.Runtime.InteropServices;

namespace BMBot.Interop.API.Process;

public static partial class ProcessInterop
{
    [LibraryImport("kernel32.dll")]
    public static partial IntPtr OpenProcess(int p_desiredAccess, [MarshalAs(UnmanagedType.Bool)] bool p_inheritHandle, int p_processId);
}