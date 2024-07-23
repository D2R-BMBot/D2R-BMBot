using System;

using BMBot.GUI.Avalonia.Models.DataStructures.Game.Instance;
using BMBot.Interop.API.Process.Memory;

namespace BMBot.GUI.Avalonia.Models.Services.Game;

public class UnitTableService
{
    public void FindPlayer(InstanceData p_instanceData)
    {
        var unitTableBuffer = new byte[( 128 + 516 ) * 8];

        var unitOffset = p_instanceData.Pointers.UnitTableAddress - 32;

        var bytesRead = 0;
        
        MemoryInterop.ReadProcessMemory(p_instanceData.ProcessHandle.ToInt32(), unitOffset, unitTableBuffer, unitTableBuffer.Length, ref bytesRead);
        
        // for (var i = 0; i < unitTableBuffer.Length; i += 8)
        // {
        //     var unitPointerLocation = BitConverter.ToInt64(unitTableBuffer, i);
        //
        //     if (unitPointerLocation > 0)
        //     {
        //         var itemDataBuffer = new byte[144];
        //         var unitBytesRead  = 0;
        //         MemoryInterop.ReadProcessMemory(p_instanceData.ProcessHandle.ToInt32(), (IntPtr)unitPointerLocation, itemDataBuffer, 144, ref unitBytesRead);
        //
        //         // Do ONLY UnitType:0 && TxtFileNo:3
        //         //if (BitConverter.ToUInt32(itemdatastruc, 0) == 0 && BitConverter.ToUInt32(itemdatastruc, 4) == 3)
        //         if (BitConverter.ToUInt32(itemDataBuffer, 0) == 0)
        //         {
        //             // PlayerStrucCount++;
        //             //Form1_0.method_1("PPointerLocation: 0x" + (UnitPointerLocation).ToString("X"));
        //
        //             var pUnitDataPtr = BitConverter.ToInt64(itemDataBuffer, 0x10);
        //             var pUnitData    = new byte[144];
        //             Form1_0.Mem_0.ReadRawMemory(pUnitDataPtr, ref pUnitData, 144);
        //
        //             var name = "";
        //             for (var i2 = 0; i2 < 16; i2++)
        //             {
        //                 if (pUnitData[i2] != 0x00)
        //                 {
        //                     name += (char)pUnitData[i2];
        //                 }
        //             }
        //             //name = name.Replace("?", "");
        //             //Form1_0.method_1("PNAME: " + name, Color.Red);
        //
        //             //Console.WriteLine(BitConverter.ToUInt32(itemdatastruc, 0));
        //             //Console.WriteLine(BitConverter.ToUInt32(itemdatastruc, 4));
        //
        //             var   ppath     = BitConverter.ToInt64(itemdatastruc, 0x38);
        //             var ppathData = new byte[144];
        //             Form1_0.Mem_0.ReadRawMemory(ppath, ref ppathData, 144);
        //
        //             //if posX equal not zero
        //             if (BitConverter.ToInt16(ppathData, 2) != 0 && name == CharConfig.PlayerCharName)
        //             {
        //                 Form1_0.method_1("------------------------------------------", Color.DarkBlue);
        //                 PlayerPointer = UnitPointerLocation;
        //                 Form1_0.Grid_SetInfos("Pointer", "0x" + PlayerPointer.ToString("X"));
        //                 FoundPlayer = true;
        //                 unitId      = BitConverter.ToUInt32(itemdatastruc, 0x08);
        //                 Form1_0.method_1("Player ID: 0x" + unitId.ToString("X"), Color.DarkBlue);
        //
        //                 /*string SavePathh = Form1_0.ThisEndPath + "DumpPlayerStruc";
        //                 File.Create(SavePathh).Dispose();
        //                 File.WriteAllBytes(SavePathh, itemdatastruc);
        //                 SavePathh = Form1_0.ThisEndPath + "DumpPlayerUnitData";
        //                 File.Create(SavePathh).Dispose();
        //                 File.WriteAllBytes(SavePathh, pUnitData);
        //                 SavePathh = Form1_0.ThisEndPath + "DumpPlayerPath";
        //                 File.Create(SavePathh).Dispose();
        //                 File.WriteAllBytes(SavePathh, ppathData);*/
        //
        //                 return;
        //             }
        //         }
        //     }
        // }
    }
}