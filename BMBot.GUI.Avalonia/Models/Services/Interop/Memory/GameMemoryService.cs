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
        var readBuffer    = new byte[p_length];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);

        if ( bytesRead != p_length )
        {
            var error = Marshal.GetLastWin32Error();
            throw new DataException($"Failed to read bytes from memory: {error}");
        }
        
        return readBuffer.AsSpan();
    }
    
    public static byte ReadByte(InstanceData p_instance, IntPtr p_address)
    {
        var readBuffer    = new byte[1];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);
        
        if (bytesRead != 1) throw new DataException("Failed to read byte from memory");
        
        return readBuffer[0];
    }
    
    public static long ReadInt64(InstanceData p_instance, IntPtr p_address)
    {
        var readBuffer    = new byte[sizeof(long)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);
        
        if (bytesRead != 8) throw new DataException("Failed to read long from memory");
        
        return BitConverter.ToInt64(readBuffer, 0);
    }
    
    public static int ReadInt32(InstanceData p_instance, IntPtr p_address)
    {
        var readBuffer    = new byte[sizeof(int)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);
        
        if (bytesRead != 4) throw new DataException("Failed to read int from memory");
        
        return BitConverter.ToInt32(readBuffer, 0);
    }

    public static uint ReadUInt32(InstanceData p_instance, IntPtr p_address)
    {
        var readBuffer = new byte[sizeof(uint)];

        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);
        
        if (bytesRead != 4) throw new DataException("Failed to read uint from memory");
        
        return BitConverter.ToUInt32(readBuffer, 0);
    }
    
    public static short ReadInt16(InstanceData p_instance, IntPtr p_address)
    {
        var readBuffer    = new byte[sizeof(short)];
        var bytesRead = 0;
        MemoryInterop.ReadProcessMemory(p_instance.ProcessHandle.ToInt32(), p_address, readBuffer, readBuffer.Length, ref bytesRead);
        
        if (bytesRead != 2) throw new DataException("Failed to read short from memory");
        
        return BitConverter.ToInt16(readBuffer, 0);
    }

    public static string ReadString(InstanceData p_instance, IntPtr p_address, int p_stringMaxLength = 16)
    {
        var charBuffer = new char[p_stringMaxLength];
        
        for (var i = 0; i < p_stringMaxLength; i++)
        {
            var currentByte = ReadByte(p_instance, p_address + i);

            // Hit null terminator, no more string to get. - Comment by M9 on 07/31/2024 @ 00:00:00
            if (currentByte == 0x00) break;
            
            charBuffer[i] = (char)currentByte;
        }
        
        return new string(charBuffer);
    }
}