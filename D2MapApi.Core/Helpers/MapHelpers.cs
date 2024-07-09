using System;
using System.Collections.Generic;
using System.Linq;

using D2MapApi.Common.DataStructures;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Common.Lookup;
using D2MapApi.Core.Wrapper;

namespace D2MapApi.Core.Helpers
{
    public static class MapHelpers
    {
        public static uint[] pActLevels = { 1, 40, 75, 103, 109, 137 };

        private static uint m_unitTypeNpc = 1;
        private static uint m_unitTypeObject = 2;
        private static uint m_unitTypeTile = 5;
        public static D2Act GetAct(D2Area p_d2Area)
        {
            for (uint i = 1; i < 5; ++i)
            {
                if ((int)p_d2Area < pActLevels[i])
                {
                    return (D2Act)(i - 1);
                }
            }
            return D2Act.ACT_5;
        }

        // public static unsafe CollisionMap BuildCollisionMap(Wrapper.Act* p_act, D2Area p_d2Area)
        // {
        //     var collisionMap = new CollisionMap();
        //     
        //     if (p_act->pActMisc->RealTombArea != 0)
        //     {
        //         collisionMap.TombArea = (D2Area)p_act->pActMisc->RealTombArea;
        //     }
        //
        //     var level = MapDll.GetLevel(p_act->pActMisc, (uint)p_d2Area);
        //
        //     if (level != null)
        //     {
        //         if (level->pRoom2First == null)
        //         {
        //             MapDll.InitLevel(level);
        //         }
        //
        //         if (level->pRoom2First != null)
        //         {
        //             collisionMap.LevelOrigin = new Point2D(level->dwPosX * 5, level->dwPosY * 5);
        //             var width = (int)level->dwSizeX * 5;
        //             var height = (int)level->dwSizeY * 5;
        //             collisionMap.Width  = width;
        //             collisionMap.Height = height;
        //             collisionMap.Map    = new List<List<int>>(height);
        //             for (var i = 0; i < height; i++)
        //             {
        //                 collisionMap.Map.Add([..Enumerable.Repeat(-1, width)]);
        //             }
        //
        //             for (var room2 = level->pRoom2First; room2 != null; room2 = room2->pRoom2Next)
        //             {
        //                 var bAdded = false;
        //
        //                 if (room2->pRoom1 == null)
        //                 {
        //                     bAdded = true;
        //                     MapDll.AddRoomData(p_act, level->dwLevelNo, room2->dwPosX, room2->dwPosY, null);
        //                 }
        //
        //                 // levels near
        //                 for (uint i = 0; i < room2->dwRoomsNear; i++)
        //                 {
        //                     if (level->dwLevelNo != room2->pRoom2Near[i]->pLevel->dwLevelNo)
        //                     {
        //                         var originX = room2->pRoom2Near[i]->pLevel->dwPosX * 5;
        //                         var originY = room2->pRoom2Near[i]->pLevel->dwPosY * 5;
        //                         var origin = new Point2D(originX, originY);
        //                         var newLevelWidth = room2->pRoom2Near[i]->pLevel->dwSizeX * 5;
        //                         var newLevelHeight = room2->pRoom2Near[i]->pLevel->dwSizeY * 5;
        //
        //                         var levelNumber = room2->pRoom2Near[i]->pLevel->dwLevelNo;
        //                         var adjacentLevel = new AdjacentLevel { LevelOrigin = origin, Width = (int)newLevelWidth, Height = (int)newLevelHeight };
        //                         collisionMap.AdjacentLevels.TryAdd(levelNumber.ToString(), adjacentLevel);
        //                     }
        //                 }
        //
        //                 // add collision data
        //                 if (room2->pRoom1 != null && room2->pRoom1->Coll != null)
        //                 {
        //                     var x = room2->pRoom1->Coll->dwPosGameX - collisionMap.LevelOrigin.X;
        //                     var y = room2->pRoom1->Coll->dwPosGameY - collisionMap.LevelOrigin.Y;
        //                     var cx = room2->pRoom1->Coll->dwSizeGameX;
        //                     var cy = room2->pRoom1->Coll->dwSizeGameY;
        //                     var nLimitX = x + cx;
        //                     var nLimitY = y + cy;
        //
        //                     var p = room2->pRoom1->Coll->pMapStart;
        //                     for (var j = y; j < nLimitY; j++)
        //                     {
        //                         for (var i = x; i < nLimitX; i++)
        //                         {
        //                             collisionMap.Map[(int)j][(int)i] = *p++;
        //                         }
        //                     }
        //                 }
        //
        //                 // add unit data
        //                 for (var presetUnit = room2->pPreset; presetUnit != null; presetUnit = presetUnit->pPresetNext)
        //                 {
        //                     // npcs
        //                     if (presetUnit->dwType == m_unitTypeNpc)
        //                     {
        //                         var npcX = room2->dwPosX * 5 + presetUnit->dwPosX;
        //                         var npcY = room2->dwPosY * 5 + presetUnit->dwPosY;
        //                         var fileNumber = presetUnit->dwTxtFileNo.ToString();
        //                         if (!collisionMap.Npcs.TryAdd(fileNumber, [new Point2D(npcX, npcY)]))
        //                         {
        //                             collisionMap.Npcs[fileNumber].Add(new Point2D(npcX, npcY));
        //                         }
        //                     }
        //
        //                     // objects
        //                     if (presetUnit->dwType == m_unitTypeObject)
        //                     {
        //                         var objectX = room2->dwPosX * 5 + presetUnit->dwPosX;
        //                         var objectY = room2->dwPosY * 5 + presetUnit->dwPosY;
        //                         var fileNumber = presetUnit->dwTxtFileNo.ToString();
        //                         if (!collisionMap.Objects.TryAdd(fileNumber, [new Point2D(objectX, objectY)]))
        //                         {
        //                             collisionMap.Objects[fileNumber].Add(new Point2D(objectX, objectY));
        //                         }
        //                     }
        //
        //                     // level exits
        //                     if (presetUnit->dwType == m_unitTypeTile)
        //                     {
        //                         for (var roomTile = room2->pRoomTiles; roomTile != null; roomTile = roomTile->pNext)
        //                         {
        //                             if (*roomTile->nNum == presetUnit->dwTxtFileNo)
        //                             {
        //                                 var exitX = room2->dwPosX * 5 + presetUnit->dwPosX;
        //                                 var exitY = room2->dwPosY * 5 + presetUnit->dwPosY;
        //
        //                                 var levelNumber = roomTile->pRoom2->pLevel->dwLevelNo.ToString();
        //                                 var areaName    = ( (D2Area)roomTile->pRoom2->pLevel->dwLevelNo ).ToFriendlyString();
        //                                 collisionMap.AdjacentLevels[levelNumber].Exits.Add(new Point2D(exitX, exitY));
        //                             }
        //                         }
        //                     }
        //                 }
        //
        //                 if (bAdded)
        //                 {
        //                     MapDll.RemoveRoomData(p_act, level->dwLevelNo, room2->dwPosX, room2->dwPosY, null);
        //                 }
        //             }
        //         }
        //     }
        //
        //     return collisionMap;
        // }
        
        public static unsafe D2AreaMap BuildAreaMap(Act* p_act, D2Area p_d2Area)
        {
            var areaMap = new D2AreaMap
                          {
                              Area = p_d2Area
                          };
            
            if (p_act->pActMisc->RealTombArea != 0)
            {
                areaMap.TombArea = (D2Area)p_act->pActMisc->RealTombArea;
            }

            var level = MapDll.GetLevel(p_act->pActMisc, (uint)p_d2Area);

            if (level != null)
            {
                if (level->pRoom2First == null)
                {
                    MapDll.InitLevel(level);
                }

                if (level->pRoom2First != null)
                {
                    areaMap.LevelOrigin = new Point2D(level->dwPosX * 5, level->dwPosY * 5);
                    var width = (int)level->dwSizeX * 5;
                    var height = (int)level->dwSizeY * 5;
                    areaMap.Width    = width;
                    areaMap.Height   = height;
                    areaMap.CollisionData = new CollisionData(width, height);

                    var accessibleAreas = new HashSet<D2Area>();

                    for (var room2 = level->pRoom2First; room2 != null; room2 = room2->pRoom2Next)
                    {
                        var bAdded = false;

                        if (room2->pRoom1 == null)
                        {
                            bAdded = true;
                            MapDll.AddRoomData(p_act, level->dwLevelNo, room2->dwPosX, room2->dwPosY, null);
                        }
                        
                        // Areas accessible from this area. - Comment by M9 on 07/05/2024 @ 08:23:19
                        for (uint i = 0; i < room2->dwRoomsNear; i++)
                        {
                            if (level->dwLevelNo != room2->pRoom2Near[i]->pLevel->dwLevelNo)
                            {
                                // Maybe need this for building act maps all at once? - Comment by M9 on 07/04/2024 @ 22:13:02
                                // var originX = room2->pRoom2Near[i]->pLevel->dwPosX * 5;
                                // var originY = room2->pRoom2Near[i]->pLevel->dwPosY * 5;

                                // var origin = new Point2D(originX, originY);
                                // var newLevelWidth = room2->pRoom2Near[i]->pLevel->dwSizeX * 5;
                                // var newLevelHeight = room2->pRoom2Near[i]->pLevel->dwSizeY * 5;

                                var levelNumber = room2->pRoom2Near[i]->pLevel->dwLevelNo;
                                accessibleAreas.Add((D2Area)levelNumber);
                            }
                        }

                        // add collision data
                        if (room2->pRoom1 != null && room2->pRoom1->Coll != null)
                        {
                            var x       = room2->pRoom1->Coll->dwPosGameX - areaMap.LevelOrigin.X;
                            var y       = room2->pRoom1->Coll->dwPosGameY - areaMap.LevelOrigin.Y;
                            var cx      = room2->pRoom1->Coll->dwSizeGameX;
                            var cy      = room2->pRoom1->Coll->dwSizeGameY;
                            var nLimitX = x + cx;
                            var nLimitY = y + cy;

                            var p = room2->pRoom1->Coll->pMapStart;
                            for (var i = y; i < nLimitY; i++)
                            {
                                for (var j = x; j < nLimitX; j++)
                                {
                                    var blockTypeId = *p;
                                    areaMap.CollisionData.Blocks[(int)j,(int)i] = (CollisionBlock)blockTypeId;
                                    p++;
                                }
                            }
                        }

                        // add unit data
                        for (var presetUnit = room2->pPreset; presetUnit != null; presetUnit = presetUnit->pPresetNext)
                        {
                            // npcs
                            if (presetUnit->dwType == m_unitTypeNpc)
                            {
                                var npcX = room2->dwPosX * 5 + presetUnit->dwPosX - areaMap.LevelOrigin.X;
                                var npcY      = room2->dwPosY * 5 + presetUnit->dwPosY- areaMap.LevelOrigin.Y;
                                var npcNumber = presetUnit->dwTxtFileNo;
                                
                                var npcId     = (D2Npc)npcNumber;

                                areaMap.Npcs.Add((npcId, new D2NpcData(npcId, new Point2D(npcX, npcY))));
                            }

                            // objects
                            if (presetUnit->dwType == m_unitTypeObject)
                            {
                                var objectX      = room2->dwPosX * 5 + presetUnit->dwPosX - areaMap.LevelOrigin.X;
                                var objectY      = room2->dwPosY * 5 + presetUnit->dwPosY - areaMap.LevelOrigin.Y;
                                var objectNumber = presetUnit->dwTxtFileNo;

                                var objectId = (D2Object)objectNumber;

                                var objectData = D2ObjectDataLookup.D2Objects[objectId];

                                objectData.SetPosition(new Point2D(objectX, objectY));
                                
                                areaMap.Objects.Add(( objectId, D2ObjectDataLookup.D2Objects[objectId] ));
                            }

                            // level exits
                            if (presetUnit->dwType == m_unitTypeTile)
                            {
                                for (var roomTile = room2->pRoomTiles; roomTile != null; roomTile = roomTile->pNext)
                                {
                                    if (*roomTile->nNum == presetUnit->dwTxtFileNo)
                                    {
                                        var exitX = room2->dwPosX * 5 + presetUnit->dwPosX - areaMap.LevelOrigin.X;
                                        var exitY = room2->dwPosY * 5 + presetUnit->dwPosY - areaMap.LevelOrigin.Y;
                                        
                                        var area    = (D2Area)roomTile->pRoom2->pLevel->dwLevelNo;
                                        var accessibleArea = accessibleAreas.FirstOrDefault(p_area => p_area == area);
                                        areaMap.AccessibleAreas.Add((accessibleArea, new Point2D(exitX, exitY)));
                                    }
                                }
                            }
                        }

                        if (bAdded)
                        {
                            MapDll.RemoveRoomData(p_act, level->dwLevelNo, room2->dwPosX, room2->dwPosY, null);
                        }
                    }
                }
            }

            return areaMap;
        }
    }
}
