using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Common.DataStructures;

public class D2NpcData(D2Npc p_d2Npc, Point2D p_position)
{
    public D2Npc   D2Npc    { get; set; } = p_d2Npc;
    public Point2D Position { get; set; } = p_position;
}