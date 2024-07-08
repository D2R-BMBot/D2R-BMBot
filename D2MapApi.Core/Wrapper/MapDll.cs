using System.Runtime.InteropServices;

namespace D2MapApi.Core.Wrapper
{
    internal static class MapDll
    {
        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        internal static extern bool Initialize(string p_path);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe Level* GetLevel(ActMisc* p_misc, uint p_levelNumber);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void InitLevel(Level* p_level);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void AddRoomData(Act* p_act, uint p_levelId, uint p_xPosition, uint p_yPosition, Room1* p_room);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void RemoveRoomData(Act* p_act, uint p_levelId, uint p_xPosition, uint p_yPosition, Room1* p_room);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe Act* LoadAct(uint p_actNumber, uint p_seed, uint p_difficulty, uint p_townLevelId);

        [DllImport("D2MapApi.DllWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern unsafe void UnloadAct(Act* p_act);

        [DllImport("kernel32.dll")]
        internal static extern uint GetLastError();
    }
}
