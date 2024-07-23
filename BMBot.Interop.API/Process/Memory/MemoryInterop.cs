using System.Runtime.InteropServices;

namespace BMBot.Interop.API.Process.Memory;

public static partial class MemoryInterop
{
    [LibraryImport("kernel32.dll", EntryPoint = "ReadProcessMemory", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReadProcessMemory(int p_processHandle, IntPtr p_startAddress, byte[] p_outputBuffer, int p_bufferSize, ref int p_numberOfBytesRead);
}