using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;
using static MapAreaStruc;

public class AncientTunnels
{
    Form1 Form1_0;
    public List<int> IgnoredChestList = new List<int>();
    public int CurrentStep = 0;
    public bool ScriptDone = false;
    public bool HasTakenAnyChest = false;
    public Position TrappDoorPos = new Position { X = 0, Y = 0 };
    public Position ChestPos = new Position { X = 0, Y = 0 };
    public void TakeChest(Enums.Area ThisArea)
    {
        MapAreaStruc.Position ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "GoodChest", (int)ThisArea, IgnoredChestList);
        int ChestObject = Form1_0.MapAreaStruc_0.CurrentObjectIndex;
        int Tryy = 0;
        while (ThisChestPos.X != 0 && ThisChestPos.Y != 0 && Tryy < 30)
        {
            if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
            {
                ScriptDone = true;
                return;
            }

            if (Form1_0.Mover_0.MoveToLocation(ThisChestPos.X, ThisChestPos.Y))
            {
                HasTakenAnyChest = true;

                Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, ThisChestPos.X, ThisChestPos.Y);

                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y - 15);
                Form1_0.WaitDelay(10);
                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y - 15);
                Form1_0.WaitDelay(10);
                Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y - 15);
                Form1_0.WaitDelay(10);

                int tryy2 = 0;
                while (Form1_0.ItemsStruc_0.GetItems(true) && tryy2 < 20)
                {
                    Form1_0.PlayerScan_0.GetPositions();
                    Form1_0.ItemsStruc_0.GetItems(false);
                    Form1_0.Potions_0.CheckIfWeUsePotion();
                    tryy2++;
                }
                IgnoredChestList.Add(ChestObject);
            }

            ThisChestPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "GoodChest", (int)ThisArea, IgnoredChestList);
            ChestObject = Form1_0.MapAreaStruc_0.CurrentObjectIndex;

            Tryy++;
        }

        if (!HasTakenAnyChest) Form1_0.MapAreaStruc_0.DumpMap();
    }
    public void SetForm1(Form1 form1_1)
    {
        Form1_0 = form1_1;
    }

    public void ResetVars()
    {
        CurrentStep = 0;
        ScriptDone = false;
    }

    public void DetectCurrentStep()
    {
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.LostCity) CurrentStep = 1;
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.AncientTunnels) CurrentStep = 2;

        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.AncientTunnels)
        {
            Form1_0.PathFinding_0.MoveToExit(Enums.Area.AncientTunnels);
            CurrentStep = 2;
        }
    }

    public void RunScript()
    {
        //Form1_0.SetGameStatus("test1");

        Form1_0.Town_0.ScriptTownAct = 2; //set to town act 2 when running this script

        if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
        {
            ScriptDone = true;
            return;
        }

        if (Form1_0.Town_0.GetInTown())
        {
            Form1_0.SetGameStatus("GO TO WP");
            CurrentStep = 0;

            Form1_0.Town_0.GoToWPArea(2, 5);
        }
        else
        {
            if (CurrentStep == 0)
            {
                Form1_0.SetGameStatus("DOING ANCIENT TUNNELS");
                Form1_0.Battle_0.CastDefense();
                Form1_0.WaitDelay(15);

                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.LostCity)
                {
                    CurrentStep++;
                }
                else
                {
                    DetectCurrentStep();
                    if (CurrentStep == 0)
                    {
                        Form1_0.Town_0.FastTowning = false;
                        Form1_0.Town_0.GoToTown();
                    }
                }
            }

            if (CurrentStep == 1)
            {
                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo != Enums.Area.LostCity)
                {
                    CurrentStep--;
                    return;
                }

                TrappDoorPos = Form1_0.MapAreaStruc_0.GetPositionOfObject("object", "TrappDoor", (int)Enums.Area.LostCity, new List<int>());

                {
                    if (TrappDoorPos.X != 0 && TrappDoorPos.Y != 0)
                    {
                        Form1_0.PathFinding_0.MoveToThisPos(TrappDoorPos);

                        int tryyy = 0;
                        while (tryyy <= 25)
                        {
                            Position itemScreenPos = Form1_0.GameStruc_0.World2Screen(Form1_0.PlayerScan_0.xPosFinal, Form1_0.PlayerScan_0.yPosFinal, TrappDoorPos.X, TrappDoorPos.Y);

                            Form1_0.KeyMouse_0.MouseClicc_RealPos(itemScreenPos.X, itemScreenPos.Y);
                            Form1_0.PlayerScan_0.GetPositions();
                            Form1_0.WaitDelay(5);
                            tryyy++;
                        }
                                                                   }
                    else
                    {
                        Form1_0.method_1("TrappDoor not detected!", Color.Red);
                        Form1_0.Town_0.FastTowning = false;
                        Form1_0.Town_0.UseLastTP = false;
                        ScriptDone = true;
                        return;
                    }

                    Form1_0.PathFinding_0.MoveToExit(Enums.Area.AncientTunnels);

                    CurrentStep++;
                    return;
                }

            }

            if (CurrentStep == 2)
            {                                              
                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo != Enums.Area.AncientTunnels)
                    {
                        CurrentStep--;
                        return;
                    }
                    
                    Form1_0.SetGameStatus("CLEARING ANCIENT TUNNELS");
                    Form1_0.WaitDelay(250);
                    if ((Enums.Area)Form1_0.Battle_0.AreaIDFullyCleared != Enums.Area.AncientTunnels)
                    {
                        Form1_0.Battle_0.ClearFullAreaOfMobs();
                        TakeChest(Enums.Area.AncientTunnels);
                    if (!Form1_0.Battle_0.ClearingArea)
                        {
                            Form1_0.Town_0.FastTowning = false;
                            Form1_0.Town_0.UseLastTP = false;
                            ScriptDone = true;
                        }
                    }
                    else
                    {
                        Form1_0.Town_0.FastTowning = false;
                        Form1_0.Town_0.UseLastTP = false;
                        ScriptDone = true;
                    }
                }
            }
        }
    }



