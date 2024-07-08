using System;

using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Core.Helpers;
using D2MapApi.Core.Wrapper;

namespace D2MapApi.Core.Models
{
    public unsafe class Session : IDisposable
    {
        public Session(uint p_seed, D2Difficulty p_d2Difficulty)
        {
            m_acts     = new Act*[5];
            Seed       = p_seed;
            D2Difficulty = p_d2Difficulty;
        }

        public uint Seed { get; }

        public D2Difficulty D2Difficulty { get; }

        private readonly Act*[] m_acts;

        public D2AreaMap GetMap(D2Area p_d2Area)
        {
            var act = MapHelpers.GetAct(p_d2Area);
            var actIndex = (int)act;
            if (m_acts[actIndex] == null)
            {
                m_acts[actIndex] = MapDll.LoadAct((uint)actIndex, Seed, (uint)D2Difficulty, MapHelpers.pActLevels[actIndex]);
            }
            return MapHelpers.BuildAreaMap(m_acts[actIndex], p_d2Area);
        }

        public void Dispose()
        {
            foreach (var act in m_acts)
            {
                if (act != null)
                {
                    MapDll.UnloadAct(act);
                }
            }
        }
    }
}
