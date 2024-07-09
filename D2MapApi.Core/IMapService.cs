using System.Threading.Tasks;

using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Core.Models;

namespace D2MapApi.Core
{
    public interface IMapService
    {
        Task               InitializeAsync(string    p_d2GameDirectoryPath);
        Task<D2AreaMap> GetCollisionMapAsync(uint p_seed, D2Difficulty p_d2Difficulty, D2Area p_d2Area);
    }
}
