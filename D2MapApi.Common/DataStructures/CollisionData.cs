using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Common.DataStructures;

public class CollisionData(int p_width, int p_height)
{
    public int               Width  { get; } = p_width;
    public int               Height { get; } = p_height;
    public CollisionBlock[,] Blocks { get; } = new CollisionBlock[p_width, p_height];
}