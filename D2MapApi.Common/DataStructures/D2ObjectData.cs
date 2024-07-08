using D2MapApi.Common.Enumerations.Extensions;
using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Common.DataStructures;

public class D2ObjectData(D2Object p_objectId, int p_width, int p_height, bool p_hasCollision)
{
    public void SetPosition(Point2D p_position) => Position = p_position;
    
    public string   Name         => ObjectId.ToFriendlyString();
    public D2Object ObjectId     { get; private set; } = p_objectId;
    public int      Width        { get; private set; } = p_width;
    public int      Height       { get; private set; } = p_height;
    public bool     HasCollision { get; private set; } = p_hasCollision;
    
    public Point2D Position { get; private set; }
}