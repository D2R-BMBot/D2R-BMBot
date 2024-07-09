using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Common.DataStructures;

public class D2AreaMap
{
    public D2Area                                             Area            { get; set; }
    public Point2D                                            LevelOrigin     { get; set; }
    public int                                                Width           { get; set; }
    public int                                                Height          { get; set; }
    public CollisionData                                      CollisionData   { get; set; } = new(0, 0);
    public List<(D2Npc NpcId, D2NpcData NpcData)>             Npcs            { get; }      = [];
    public List<(D2Object ObjectId, D2ObjectData ObjectData)> Objects         { get; }      = [];
    public List<(D2Area Area, Point2D ExitPosition)>          AccessibleAreas { get; }      = [];
    public D2Area                                             TombArea        { get; set; }
}