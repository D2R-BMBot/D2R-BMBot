namespace D2MapApi.Common.Enumerations.GameData;

public enum CollisionBlock
{
    // VOID = -1,
    // NO_COLLISION = 0,
    // COLLISION = 1
    NONE                = 0x0000,
    BLOCK_WALK          = 0x0001,
    BLOCK_LINE_OF_SIGHT = 0x0002,
    WALL                = 0x0004,
    BLOCK_PLAYER        = 0x0008,
    ALTERNATE_TILE      = 0x0010,
    BLANK               = 0x0020,
    MISSILE             = 0x0040,
    PLAYER              = 0x0080,
    NPC_LOCATION        = 0x0100,
    ITEM                = 0x0200,
    OBJECT              = 0x0400,
    CLOSED_DOOR         = 0x0800,
    NPC_COLLISION       = 0x1000,
    FRIENDLY_NPC        = 0x2000,
    UNKNOWN             = 0x4000,
    DEAD_BODY           = 0x8000, // also portal
    THICKENED_WALL      = 0xfefe,
    AVOID               = 0xffff
}