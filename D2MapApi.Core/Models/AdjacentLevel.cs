using System.Collections.Generic;

using D2MapApi.Common.DataStructures;

namespace D2MapApi.Core.Models
{
    public class AdjacentLevel
    {
        public List<Point2D> Exits       { get; set; } = [];
        public Point2D       LevelOrigin { get; set; }
        public int         Width       { get; set; }
        public int         Height      { get; set; }
    }
}
