using System.Collections.Generic;
using System.Text.Json.Serialization;

using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Core.Models
{
    public class CollisionMap
    {
        public                               Point2D                           LevelOrigin    { get; set; }
        [JsonPropertyName("mapRows")] public List<List<int>>                   Map            { get; set; }
        public                               int                               Width          { get; set; }
        public                               int                               Height         { get; set; }
        public                               Dictionary<string, AdjacentLevel> AdjacentLevels { get; init; } = new();
        public                               Dictionary<string, List<Point2D>> Npcs           { get; init; } = new();
        public                               Dictionary<string, List<Point2D>> Objects        { get; init; } = new();
        public                               D2Area?                             TombArea       { get; set; }
    };
}