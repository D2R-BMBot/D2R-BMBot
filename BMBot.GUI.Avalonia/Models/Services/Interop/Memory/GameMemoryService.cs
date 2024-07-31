using System;
using System.Data;
using System.Runtime.InteropServices;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;
using BMBot.Interop.API.Process.Memory;

namespace BMBot.GUI.Avalonia.Models.Services.Interop.Memory;

public static class GameMemoryService
{
    public static Span<byte> GetMemorySpan(InstanceData p_instance, IntPtr p_address, int p_length)
    {
        var buffer    = new byte[p_length];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, buffer, buffer.Length, ref bytesRead);

        if ( bytesRead != p_length )
        {
            var error = Marshal.GetLastWin32Error();
            throw new DataException($"Failed to read bytes from memory: {error}");
        }
        
        return buffer.AsSpan();
    }
    
    public static byte ReadByte(InstanceData p_instance, IntPtr p_address)
    {
        var buffer    = new byte[1];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, buffer, buffer.Length, ref bytesRead);
        
        if (bytesRead != 1) throw new DataException("Failed to read byte from memory");
        
        return buffer[0];
    }
    
    public static long ReadInt64(InstanceData p_instance, IntPtr p_address)
    {
        var buffer    = new byte[sizeof(long)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, buffer, buffer.Length, ref bytesRead);
        
        if (bytesRead != 8) throw new DataException("Failed to read long from memory");
        
        return BitConverter.ToInt64(buffer, 0);
    }
    
    public static int ReadInt32(InstanceData p_instance, IntPtr p_address)
    {
        var buffer    = new byte[sizeof(int)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, buffer, buffer.Length, ref bytesRead);
        
        if (bytesRead != 4) throw new DataException("Failed to read int from memory");
        
        return BitConverter.ToInt32(buffer, 0);
    }
    
    public static short ReadInt16(InstanceData p_instance, IntPtr p_address)
    {
        var buffer    = new byte[sizeof(short)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, buffer, buffer.Length, ref bytesRead);
        
        if (bytesRead != 2) throw new DataException("Failed to read short from memory");
        
        return BitConverter.ToInt16(buffer, 0);
    }

    public static string ReadString(InstanceData p_instance, IntPtr p_address, int p_stringMaxLength = 16)
    {
        var returnString = string.Empty;
        
        for (var i = 0; i < p_stringMaxLength; i++)
        {
            var currentByte = ReadByte(p_instance, p_address + i);
            
            if (currentByte == 0x00) break;
            
            returnString += (char)currentByte;
        }
        
        return returnString;
    }
}