﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;
using static MapAreaStruc;

public class Summoner
{
    Form1 Form1_0;

    public int CurrentStep = 0;
    public bool ScriptDone = false;

    public List<Area> TerrorZonesAreas = new List<Area>();
    public int CurrentTerrorZonesIndex = 0;


    public void SetForm1(Form1 form1_1)
    {
        Form1_0 = form1_1;
    }

    public void ResetVars()
    {
        CurrentStep = 0;
        ScriptDone = false;
        TerrorZonesAreas = new List<Area>();
        CurrentTerrorZonesIndex = 0;
    }

    public void RunScript()
    {
        Form1_0.SetGameStatus("Summoner Script");

        Form1_0.Town_0.ScriptTownAct = 2; //set to town act 5 when running this script
        
        //if (TerrorZonesAreas.Count == 0) TerrorZonesAreas = Form1_0.GameStruc_0.GetTerrorZones();

        if (!Form1_0.Running || !Form1_0.GameStruc_0.IsInGame())
        {
            ScriptDone = true;
            return;
        }

        if (Form1_0.Town_0.GetInTown())
        {
            //Form1_0.SetGameStatus("GO TO WP");
            CurrentStep = 0;

            Form1_0.Town_0.GoToWPArea(2, 7);
        }
        else
        {
            if (CurrentStep == 0)
            {
                Form1_0.SetGameStatus("Moving to Summoner");
                //Form1_0.PathFinding_0.MoveToThisPos(new Position { X = 25439, Y = 5446 });
                Form1_0.Battle_0.CastDefense();
                Form1_0.WaitDelay(200);

                if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.ArcaneSanctuary)
                {
                    CurrentStep++;
                }
                else
                {
                    Form1_0.Town_0.FastTowning = false;
                    Form1_0.Town_0.GoToTown();
                }
            }

            if (CurrentStep == 1)
            {
                Form1_0.WaitDelay(50);
                Form1_0.PathFinding_0.MoveToThisPos(new Position { X = 25439, Y = 5446 });
                Form1_0.WaitDelay(50);

                Form1_0.PathFinding_0.MoveToNPC("Summoner");
                
                if (CharConfig.RunningOnChar == "Sorceress")
                {
                    Form1_0.Battle_0.SetSkillsStatic();
                    CurrentStep++;
                }
                else
                {
                    CurrentStep++;
                    return;
                }
            }

            if (CurrentStep == 2)
            {
                Form1_0.Potions_0.CanUseSkillForRegen = false;
                Form1_0.SetGameStatus("KILLING SUMMONER");
                Form1_0.MobsStruc_0.DetectThisMob("getBossName", "Summoner", false, 200, new List<long>());
                if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Summoner", false, 200, new List<long>()))
                {
                    if (Form1_0.MobsStruc_0.MobsHP > 0)
                    {
                        Form1_0.Battle_0.RunBattleScriptOnThisMob("getBossName", "Summoner", new List<long>());
                    }
                    else
                    {
                        if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
                        return;
                    }
                }
                else
                {
                    Form1_0.method_1("Summoner not detected!", Color.Red);

                    //baal not detected...
                    Form1_0.ItemsStruc_0.GetItems(true);
                    if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Summoner", false, 200, new List<long>())) return; //redetect baal?
                    Form1_0.ItemsStruc_0.GrabAllItemsForGold();
                    if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Summoner", false, 200, new List<long>())) return; //redetect baal?
                    Form1_0.Potions_0.CanUseSkillForRegen = true;

                    if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
                    return;
                }
            }
        }
    }
}
