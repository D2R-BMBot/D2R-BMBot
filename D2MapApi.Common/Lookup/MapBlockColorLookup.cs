using System.Numerics;

namespace D2MapApi.Common.Lookup;

public static class MapBlockColorLookup
{
    public static Dictionary<int, Vector4> ColorLookup { get; } = new()
    {
        { -1, new Vector4(0, 0, 0, 1)},              // Void: Black
        { 0, new Vector4(0.75f, 0.75f, 0.75f, 1) },  // No Collision: Light Gray
        { 1, new Vector4(0, 0, 0, 1) },              // General Collision: Black
        { 5, new Vector4(0.2f, 0.2f, 0.2f, 1) },     // Walls: Dark Gray
        { 21, new Vector4(0.2f, 0.2f, 0.2f, 1) },    // Feature Walls: Dark Gray
        { 16, new Vector4(0.75f, 0.75f, 0.75f, 1) }, // Ground Patches: Light Gray
    };
}