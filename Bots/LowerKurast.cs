﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app
{
    public class LowerKurast
    {
        Form1 Form1_0;

        public int CurrentStep = 0;
        public int WP_X = 0;
        public int WP_Y = 0;
        public List<int> IgnoredChestList = new List<int>();
        public bool ScriptDone = false;

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        public void ResetVars()
        {
            CurrentStep = 0;
            WP_X = 0;
            WP_Y = 0;
            IgnoredChestList = new List<int>();
            ScriptDone = false;
        }

        public void RunScript()
        {
            Form1_0.Town_0.ScriptTownAct = 5; //set to town act 5 when running this script

            if (Form1_0.Town_0.GetInTown())
            {
                Form1_0.SetGameStatus("GO TO WP");
                CurrentStep = 0;

                Form1_0.Town_0.GoToWPArea(3, 4);
                //close to store spot 5078, 5026
                /*if (Form1_0.Town_0.IsPosCloseTo(5084, 5037, 7))
                {
                    //move close to tp location
                    Form1_0.Mover_0.MoveToLocation(5103, 5029);
                }
                else
                {
                    //move close to wp location
                    if (Form1_0.Mover_0.MoveToLocation(5119, 5067))
                    {
                        //use wp
                        //if (Form1_0.ObjectsStruc_0.GetObjects("ExpansionWaypoint", false))
                        //{
                        //Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, Form1_0.ObjectsStruc_0.itemx, Form1_0.ObjectsStruc_0.itemy);
                        Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, 5114, 5069);
                        Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                        Form1_0.Mover_0.FinishMoving();
                        if (Form1_0.UIScan_0.WaitTilUIOpen("waypointMenu"))
                        {
                            Form1_0.KeyMouse_0.MouseClicc(415, 220); //select act3
                            Form1_0.WaitDelay(50);
                            Form1_0.KeyMouse_0.MouseClicc(400, 515); //select kurast wp
                            Form1_0.UIScan_0.WaitTilUIClose("waypointMenu");
                            Form1_0.UIScan_0.WaitTilUIClose("loading");
                            Form1_0.WaitDelay(350);
                        }
                        //}
                    }
                }*/
            }
            else
            {
                if (CurrentStep == 0)
                {
                    Form1_0.SetGameStatus("DOING KURAST");
                    Form1_0.Battle_0.CastDefense();
                    Form1_0.WaitDelay(15);

                    if (Form1_0.PlayerScan_0.levelNo == 79)
                    {
                        WP_X = Form1_0.PlayerScan_0.xPos - 3;
                        WP_Y = Form1_0.PlayerScan_0.yPos - 3;

                        CurrentStep++;
                    }
                    else
                    {
                        Form1_0.Town_0.GoToTown();
                    }
                }

                if (CurrentStep == 1)
                {
                    TakeChest();

                    CurrentStep++;
                }

                if (CurrentStep == 2)
                {
                    //Form1_0.ItemsStruc_0.GrabAllItemsForGold();

                    //Form1_0.LeaveGame(true);

                    if (Form1_0.Mover_0.MoveToLocation(WP_X, WP_Y))
                    {
                        //take back wp
                        //if (Form1_0.ObjectsStruc_0.GetObjects("Act3TownWaypoint", false))
                        //{
                            Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, WP_X, WP_Y);
                            Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                            //Form1_0.Mover_0.FinishMoving();
                            if (Form1_0.UIScan_0.WaitTilUIOpen("waypointMenu"))
                            {
                                Form1_0.Town_0.SelectTownWP();
                                //Form1_0.Town_0.Towning = true;
                                ScriptDone = true;

                                Form1_0.LeaveGame(true); //#####
                            }
                        //}
                    }
                }
            }
        }

        public void TakeChest()
        {
            //JungleStashObject2
            //JungleStashObject3
            //GoodChest
            //NotSoGoodChest
            //DeadVillager1
            //DeadVillager2
            //NotSoGoodChest
            //HollowLog

            //JungleMediumChestLeft ####

            MapAreaStruc.Position ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "GoodChest", (int)Enums.Area.LowerKurast, IgnoredChestList);
            int Tryy = 0;
            while (ThisChestPos.X != 0 && ThisChestPos.Y != 0 && Tryy < 30)
            {
                if (Form1_0.Mover_0.MoveToLocation(ThisChestPos.X, ThisChestPos.Y))
                {
                    Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisChestPos.X, ThisChestPos.Y);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);

                    int tryy2 = 0;
                    while (Form1_0.ItemsStruc_0.GetItems(true) && tryy2 < 20)
                    {
                        Form1_0.PlayerScan_0.GetPositions();
                        Form1_0.ItemsStruc_0.GetItems(false);
                        Form1_0.Potions_0.CheckIfWeUsePotion();
                        tryy2++;
                    }
                    IgnoredChestList.Add(Form1_0.MapAreaStruc_0.CurrentObjectIndex);
                }

                ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "GoodChest", (int) Enums.Area.LowerKurast, IgnoredChestList);

                Tryy++;
            }

            //##############
            /*ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "JungleStashObject2", 78, IgnoredChestList);
            Tryy = 0;
            while (ThisChestPos.X != 0 && ThisChestPos.Y != 0 && Tryy < 30)
            {
                if (Form1_0.Mover_0.MoveToLocation(ThisChestPos.X, ThisChestPos.Y))
                {
                    Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisChestPos.X, ThisChestPos.Y);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);

                    int tryy2 = 0;
                    while (Form1_0.ItemsStruc_0.GetItems(true) && tryy2 < 20)
                    {
                        Form1_0.PlayerScan_0.GetPositions();
                        Form1_0.ItemsStruc_0.GetItems(false);
                        Form1_0.Potions_0.CheckIfWeUsePotion();
                        tryy2++;
                    }
                    IgnoredChestList.Add(Form1_0.MapAreaStruc_0.CurrentObjectIndex);
                }

                ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "JungleStashObject2", 78, IgnoredChestList);

                Tryy++;
            }
            //##############
            ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "JungleStashObject3", 78, IgnoredChestList);
            Tryy = 0;
            while (ThisChestPos.X != 0 && ThisChestPos.Y != 0 && Tryy < 30)
            {
                if (Form1_0.Mover_0.MoveToLocation(ThisChestPos.X, ThisChestPos.Y))
                {
                    Dictionary<string, int> itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisChestPos.X, ThisChestPos.Y);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);
                    Form1_0.KeyMouse_0.MouseClicc(itemScreenPos["x"], itemScreenPos["y"] - 15);
                    Form1_0.WaitDelay(10);

                    int tryy2 = 0;
                    while (Form1_0.ItemsStruc_0.GetItems(true) && tryy2 < 20)
                    {
                        Form1_0.PlayerScan_0.GetPositions();
                        Form1_0.ItemsStruc_0.GetItems(false);
                        Form1_0.Potions_0.CheckIfWeUsePotion();
                        tryy2++;
                    }
                    IgnoredChestList.Add(Form1_0.MapAreaStruc_0.CurrentObjectIndex);
                }

                ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "JungleStashObject3", 78, IgnoredChestList);

                Tryy++;
            }*/
        }

    }
}