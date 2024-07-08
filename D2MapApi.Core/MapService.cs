using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.Extensions;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Core.Models;
using D2MapApi.Core.Wrapper;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace D2MapApi.Core
{
    public class MapService(ILogger<MapService> i_logger,
                            IMemoryCache        i_cache) : IMapService
    {
        private bool          IsInitialized { get; set; }
        private SemaphoreSlim SyncSemaphore { get; } = new (1);

        public async Task InitializeAsync(string p_d2GameDirectoryPath)
        {
            i_logger.LogInformation("Initializing map service dlls");
            
            if ( !Directory.Exists(p_d2GameDirectoryPath) )
            {
                throw new DirectoryNotFoundException();
            }

            MapDll.Initialize(p_d2GameDirectoryPath);

            IsInitialized = true;

            i_logger.LogInformation("Map service initialized successfully");
            
            await Task.CompletedTask;
        }
        
        public async Task<D2AreaMap> GetCollisionMapAsync(uint p_seed, D2Difficulty p_d2Difficulty, D2Area p_d2Area)
        {
            i_logger.LogInformation("Requesting map info for map '{Area}' with seed '{Seed}' on difficulty '{Difficulty}'", p_d2Area.ToFriendlyString(), p_seed, p_d2Difficulty.ToFriendlyString());
            
            if ( !IsInitialized )
            {
                throw new InvalidOperationException("The Map Service has not been initialized");
            }
            
            var session = i_cache.GetOrCreate(Tuple.Create("map", p_seed, p_d2Difficulty), p_cacheEntry =>
            {
                p_cacheEntry.RegisterPostEvictionCallback(DeleteSession);
                p_cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(5);
                return new Session(p_seed, p_d2Difficulty);
            });

            try
            {
                await SyncSemaphore.WaitAsync();
                return session?.GetMap(p_d2Area) ?? throw new NullReferenceException();
            }
            finally
            {
                SyncSemaphore.Release();
            }
        }

        private void DeleteSession(object p_key, object? p_value, EvictionReason p_reason, object? p_state)
        {
            if (p_value is IDisposable item)
            {
                item.Dispose();
            }
        }
    }
}
