using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Andariel : IBot
{
    public const string scriptName = "Andariel";
    public const string scriptType = "Bot";

    public string ScriptName => scriptName;
    public string ScriptType => scriptType;
    public int CurrentStep { get; set; } = 0;
    public bool ScriptDone { get; set; } = false;

    Form1 Form1_0;

    public bool DetectedBoss = false;

    public void SetForm1(Form1 form1_1)
    {
        Form1_0 = form1_1;
    }

    public void ResetVars()
    {
        CurrentStep = 0;
        ScriptDone = false;
        DetectedBoss = false;
    }

    public void DetectCurrentStep()
    {
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.CatacombsLevel2) CurrentStep = 1;
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.CatacombsLevel3) CurrentStep = 2;
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo == Enums.Area.CatacombsLevel4) CurrentStep = 3;
    }

    public void RunScript()
    {
        Form1_0.Town_0.ScriptTownAct = 1; //set to town act 5 when running this script
        //Pather.useWaypoint(sdk.areas.CatacombsLvl2);
        //Precast.doPrecast(true);

        //if (!Pather.moveToExit([sdk.areas.CatacombsLvl3, sdk.areas.CatacombsLvl4], true))
        //{
        //    throw new Error("Failed to move to Catacombs Level 4");
        //}
        
        // Go to town if not in town
        if (!Form1_0.Town_0.GetInTown())
        {
            Form1_0.method_1("weTPinthisbitch",Color.Red);
            Form1_0.Town_0.SpawnTP(true);
        }

       
        Form1_0.SetGameStatus("GO TO WP");
        Form1_0.Town_0.GoToWPArea(1, 8); // At catacombs 2
        Form1_0.WaitDelay(100); // wait for go to TP
        Form1_0.SetGameStatus("DOING ANDARIEL");
        Form1_0.method_1("doinit", Color.Red);
        ScriptHelper.UpdateLocation(Form1_0);
        Form1_0.Battle_0.CastDefense();
        Form1_0.WaitDelay(10);

        // Make sure we made it to the first map
        if ((Enums.Area)Form1_0.PlayerScan_0.levelNo != Enums.Area.CatacombsLevel2)
        {
            Form1_0.method_1($"Current map is {Form1_0.PlayerScan_0.levelNo}", Color.Red);
            Form1_0.method_1("Failed to enter Catacombs Level 2", Color.Red);

            Form1_0.Town_0.FastTowning = false;
            Form1_0.Town_0.GoToTown();
        }

        //TESTME
        //Enums.Area[] areaPath = { Enums.Area.CatacombsLevel3, Enums.Area.CatacombsLevel4 };

        Form1_0.PathFinding_0.MoveToExit(Enums.Area.CatacombsLevel3);
        Form1_0.WaitDelay(200);
        ScriptHelper.UpdateLocation(Form1_0);
        Form1_0.PathFinding_0.MoveToExit(Enums.Area.CatacombsLevel4);
        Form1_0.WaitDelay(200);
        ScriptHelper.UpdateLocation(Form1_0);
        Form1_0.WaitDelay(500);


        Form1_0.Mover_0.MoveToLocation(22549, 9520);
        KillAndariel();
    }

    public void KillAndariel()
    {
        Form1_0.Potions_0.CanUseSkillForRegen = false;
        Form1_0.SetGameStatus("KILLING ANDARIEL");

        //#############
        Form1_0.MobsStruc_0.DetectThisMob("getBossName", "Andariel", false, 200, new List<long>());
        bool DetectedAndy = Form1_0.MobsStruc_0.GetMobs("getBossName", "Andariel", false, 200, new List<long>());
        Form1_0.method_1($"Detected Andy: {DetectedAndy}", Color.Red);
        DateTime StartTime = DateTime.Now;
        TimeSpan TimeSinceDetecting = DateTime.Now - StartTime;
        while (!DetectedAndy && TimeSinceDetecting.TotalSeconds < 5)
        {
            Form1_0.SetGameStatus("WAITING DETECTING ANDARIEL");
            DetectedAndy = Form1_0.MobsStruc_0.GetMobs("getBossName", "Andariel", false, 200, new List<long>());
            TimeSinceDetecting = DateTime.Now - StartTime;

            //cast attack during this waiting time
            Form1_0.Battle_0.SetSkills();
            Form1_0.Battle_0.CastSkillsNoMove();
            Form1_0.ItemsStruc_0.GetItems(true);      //#############
            Form1_0.Potions_0.CheckIfWeUsePotion();

            if (!Form1_0.GameStruc_0.IsInGame() || !Form1_0.Running)
            {
                Form1_0.overlayForm.ResetMoveToLocation();
                return;
            }
        }
        //#############

        if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Andariel", false, 200, new List<long>()))
        {
            Form1_0.SetGameStatus("KILLING ANDARIEL");
            if (Form1_0.MobsStruc_0.MobsHP > 0)
            {
                DetectedBoss = true;
                Form1_0.Battle_0.RunBattleScriptOnThisMob("getBossName", "Andariel", new List<long>());
            }
            else
            {
                if (!DetectedBoss)
                {
                    Form1_0.method_1("Andariel not detected!", Color.Red);
                    Form1_0.Battle_0.DoBattleScript(15);
                }

                if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
                return;
            }
        }
        else
        {
            Form1_0.method_1("Andariel not detected!", Color.Red);

            Form1_0.Battle_0.DoBattleScript(15);

            //baal not detected...
            Form1_0.ItemsStruc_0.GetItems(true);
            if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Andariel", false, 200, new List<long>())) return; //redetect baal?
            Form1_0.ItemsStruc_0.GrabAllItemsForGold();
            if (Form1_0.MobsStruc_0.GetMobs("getBossName", "Andariel", false, 200, new List<long>())) return; //redetect baal?
            Form1_0.Potions_0.CanUseSkillForRegen = true;

            if (Form1_0.Battle_0.EndBossBattle()) ScriptDone = true;
            return;
        }
    }
}
