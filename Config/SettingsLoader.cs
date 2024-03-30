﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app
{
    public class SettingsLoader
    {
        Form1 Form1_0;

        public string File_PaladinHammer = Application.StartupPath + @"\Settings\Char\PaladinHammer.txt";
        public string File_SorceressBlizzard = Application.StartupPath + @"\Settings\Char\SorceressBlizzard.txt";

        public string File_CharSettings = Application.StartupPath + @"\Settings\CharSettings.txt";
        public string File_BotSettings = Application.StartupPath + @"\Settings\BotSettings.txt";
        public string File_ItemsSettings = Application.StartupPath + @"\Settings\ItemsSettings.txt";
        public string File_CubingSettings = Application.StartupPath + @"\Settings\CubingRecipes.txt";
        public string File_Settings = Application.StartupPath + @"\Settings\Settings.txt";
        string[] AllLines = new string[] { };

        public void SetForm1(Form1 form1_1)
        {
            Form1_0 = form1_1;
        }

        public void LoadSettings()
        {
            if (File.Exists(File_CharSettings))
            {
                AllLines = File.ReadAllLines(File_CharSettings);
                LoadCharSettings();
            }
            else
            {
                Form1_0.method_1("UNABLE TO FIND 'CharSettings.txt' FILE!", Color.Red);
            }

            //###################
            ReloadCharSettings();
            //###################

            //#####
            if (File.Exists(File_BotSettings))
            {
                AllLines = File.ReadAllLines(File_BotSettings);
                LoadBotSettings();
            }
            else
            {
                Form1_0.method_1("UNABLE TO FIND 'BotSettings.txt' FILE!", Color.Red);
            }
            //#####
            if (File.Exists(File_ItemsSettings))
            {
                AllLines = File.ReadAllLines(File_ItemsSettings);
                LoadItemsSettings();
            }
            else
            {
                Form1_0.method_1("UNABLE TO FIND 'ItemsSettings.txt' FILE!", Color.Red);
            }
            //#####
            if (File.Exists(File_CubingSettings))
            {
                AllLines = File.ReadAllLines(File_CubingSettings);
                LoadCubingSettings();
            }
            else
            {
                Form1_0.method_1("UNABLE TO FIND 'CubingRecipes.txt' FILE!", Color.Red);
            }
            //#####
            if (File.Exists(File_Settings))
            {
                AllLines = File.ReadAllLines(File_Settings);
                LoadOthersSettings();
            }
            else
            {
                SaveOthersSettings();
            }
        }

        public void LoadOthersSettings()
        {
            try
            {
                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            if (AllLines[i].Contains("="))
                            {
                                string[] Params = AllLines[i].Split('=');

                                if (Params[0].Contains("RunNumber"))
                                {
                                    Form1_0.CurrentGameNumber = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("D2_LOD_113C_Path"))
                                {
                                    Form1_0.D2_LOD_113C_Path = Params[1];
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD 'Settings.txt' FILE!", Color.Red);
            }
        }

        public void ReloadCharSettings()
        {
            if (CharConfig.RunningOnChar == "PaladinHammer")
            {
                if (File.Exists(File_PaladinHammer))
                {
                    AllLines = File.ReadAllLines(File_PaladinHammer);
                    LoadCurrentCharSettings();
                }
                else
                {
                    Form1_0.method_1("UNABLE TO FIND 'PaladinHammer.txt' FILE!", Color.Red);
                }
            }
            else if (CharConfig.RunningOnChar == "SorceressBlizzard")
            {
                if (File.Exists(File_SorceressBlizzard))
                {
                    AllLines = File.ReadAllLines(File_SorceressBlizzard);
                    LoadCurrentCharSettings();
                }
                else
                {
                    Form1_0.method_1("UNABLE TO FIND 'SorceressBlizzard.txt' FILE!", Color.Red);
                }
            }
        }

        public void SaveCharSettings()
        {
            string ThisFilePath = "";
            if (CharConfig.RunningOnChar == "PaladinHammer") ThisFilePath = File_PaladinHammer;
            if (CharConfig.RunningOnChar == "SorceressBlizzard") ThisFilePath = File_SorceressBlizzard;

            string[] AllLines = File.ReadAllLines(ThisFilePath);
            for (int i = 0; i < AllLines.Length; i++)
            {
                if (AllLines[i].Contains("="))
                {
                    string[] Splitted = AllLines[i].Split('=');
                    if (Splitted[0] == "KeySkillAttack") AllLines[i] = "KeySkillAttack=" + CharConfig.KeySkillAttack;
                    if (Splitted[0] == "KeySkillAura") AllLines[i] = "KeySkillAura=" + CharConfig.KeySkillAura;
                    if (Splitted[0] == "KeySkillfastMoveAtTown") AllLines[i] = "KeySkillfastMoveAtTown=" + CharConfig.KeySkillfastMoveAtTown;
                    if (Splitted[0] == "KeySkillfastMoveOutsideTown") AllLines[i] = "KeySkillfastMoveOutsideTown=" + CharConfig.KeySkillfastMoveOutsideTown;
                    if (Splitted[0] == "KeySkillDefenseAura") AllLines[i] = "KeySkillDefenseAura=" + CharConfig.KeySkillDefenseAura;
                    if (Splitted[0] == "KeySkillCastDefense") AllLines[i] = "KeySkillCastDefense=" + CharConfig.KeySkillCastDefense;
                    if (Splitted[0] == "KeySkillLifeAura") AllLines[i] = "KeySkillLifeAura=" + CharConfig.KeySkillLifeAura;

                    if (Splitted[0] == "BeltPotTypeToHave") AllLines[i] = "BeltPotTypeToHave=" + CharConfig.BeltPotTypeToHave[0] + "," + CharConfig.BeltPotTypeToHave[1] + "," + CharConfig.BeltPotTypeToHave[2] + "," + CharConfig.BeltPotTypeToHave[3];

                    /*InventoryDontCheckItem=
                    {
                    0,0,0,0,0,0,0,1,1,1,
                    0,0,0,0,0,0,0,1,1,1,
                    0,0,0,0,0,0,0,1,1,1,
                    0,0,0,0,0,0,0,1,1,1
                    }*/
                    if (Splitted[0] == "InventoryDontCheckItem")
                    {
                        string InventoryTxtt = "";
                        InventoryTxtt += Environment.NewLine + "{" + Environment.NewLine;
                        for (int w = 0; w < 40; w++)
                        {
                            if (w == 10) InventoryTxtt += Environment.NewLine;
                            if (w == 20) InventoryTxtt += Environment.NewLine;
                            if (w == 30) InventoryTxtt += Environment.NewLine;

                            InventoryTxtt += CharConfig.InventoryDontCheckItem[w];

                            if (w < 40 - 1) InventoryTxtt += ",";
                        }
                        AllLines[i] = "InventoryDontCheckItem=" + InventoryTxtt;
                    }

                    if (Splitted[0] == "PlayerCharName") AllLines[i] = "PlayerCharName=" + CharConfig.PlayerCharName;
                    if (Splitted[0] == "UseTeleport") AllLines[i] = "UseTeleport=" + CharConfig.UseTeleport;
                    if (Splitted[0] == "ChickenHP") AllLines[i] = "ChickenHP=" + CharConfig.ChickenHP;
                    if (Splitted[0] == "TakeHPPotUnder" && !Splitted[0].Contains("MercTakeHPPotUnder")) AllLines[i] = "TakeHPPotUnder=" + CharConfig.TakeHPPotUnder;
                    if (Splitted[0] == "TakeRVPotUnder") AllLines[i] = "TakeRVPotUnder=" + CharConfig.TakeRVPotUnder;
                    if (Splitted[0] == "TakeManaPotUnder") AllLines[i] = "TakeManaPotUnder=" + CharConfig.TakeManaPotUnder;
                    if (Splitted[0] == "GambleAboveGoldAmount") AllLines[i] = "GambleAboveGoldAmount=" + CharConfig.GambleAboveGoldAmount;
                    if (Splitted[0] == "GambleUntilGoldAmount") AllLines[i] = "GambleUntilGoldAmount=" + CharConfig.GambleUntilGoldAmount;
                    if (Splitted[0] == "PlayerAttackWithRightHand") AllLines[i] = "PlayerAttackWithRightHand=" + CharConfig.PlayerAttackWithRightHand;

                    if (Splitted[0] == "KeysLocationInInventory") AllLines[i] = "KeysLocationInInventory=" + CharConfig.KeysLocationInInventory.Item1 + "," + CharConfig.KeysLocationInInventory.Item2;

                    if (Splitted[0] == "UsingMerc") AllLines[i] = "UsingMerc=" + CharConfig.UsingMerc;
                    if (Splitted[0] == "MercTakeHPPotUnder") AllLines[i] = "MercTakeHPPotUnder=" + CharConfig.MercTakeHPPotUnder;
                }
            }
        }

        public void SaveCurrentSettings()
        {
            string[] AllLines = File.ReadAllLines(File_BotSettings);

            for (int i = 0; i < AllLines.Length; i++)
            {
                if (AllLines[i].Contains("="))
                {
                    string[] Splitted = AllLines[i].Split('=');
                    if (Splitted[0] == "MaxGameTime") AllLines[i] = "MaxGameTime=" + CharConfig.MaxGameTime;

                    if (Splitted[0] == "IsRushing") AllLines[i] = "IsRushing=" + CharConfig.IsRushing;
                    if (Splitted[0] == "RushLeecherName") AllLines[i] = "RushLeecherName=" + CharConfig.RushLeecherName;

                    if (Splitted[0] == "RunDarkWoodRush") AllLines[i] = "RunDarkWoodRush=" + CharConfig.RunDarkWoodRush;
                    if (Splitted[0] == "RunTristramRush") AllLines[i] = "RunTristramRush=" + CharConfig.RunTristramRush;
                    if (Splitted[0] == "RunAndarielRush") AllLines[i] = "RunAndarielRush=" + CharConfig.RunAndarielRush;
                    if (Splitted[0] == "RunRadamentRush") AllLines[i] = "RunRadamentRush=" + CharConfig.RunRadamentRush;
                    if (Splitted[0] == "RunHallOfDeadRush") AllLines[i] = "RunHallOfDeadRush=" + CharConfig.RunHallOfDeadRush;
                    if (Splitted[0] == "RunFarOasisRush") AllLines[i] = "RunFarOasisRush=" + CharConfig.RunFarOasisRush;
                    if (Splitted[0] == "RunLostCityRush") AllLines[i] = "RunLostCityRush=" + CharConfig.RunLostCityRush;
                    if (Splitted[0] == "RunSummonerRush") AllLines[i] = "RunSummonerRush=" + CharConfig.RunSummonerRush;
                    if (Splitted[0] == "RunDurielRush") AllLines[i] = "RunDurielRush=" + CharConfig.RunDurielRush;
                    if (Splitted[0] == "RunKahlimEyeRush") AllLines[i] = "RunKahlimEyeRush=" + CharConfig.RunKahlimEyeRush;
                    if (Splitted[0] == "RunKahlimBrainRush") AllLines[i] = "RunKahlimBrainRush=" + CharConfig.RunKahlimBrainRush;
                    if (Splitted[0] == "RunKahlimHeartRush") AllLines[i] = "RunKahlimHeartRush=" + CharConfig.RunKahlimHeartRush;
                    if (Splitted[0] == "RunTravincalRush") AllLines[i] = "RunTravincalRush=" + CharConfig.RunTravincalRush;
                    if (Splitted[0] == "RunMephistoRush") AllLines[i] = "RunMephistoRush=" + CharConfig.RunMephistoRush;

                    if (Splitted[0] == "RunPindleskinScript") AllLines[i] = "RunPindleskinScript=" + CharConfig.RunPindleskinScript;
                    if (Splitted[0] == "RunDurielScript") AllLines[i] = "RunDurielScript=" + CharConfig.RunDurielScript;
                    if (Splitted[0] == "RunSummonerScript") AllLines[i] = "RunSummonerScript=" + CharConfig.RunSummonerScript;
                    if (Splitted[0] == "RunMephistoScript") AllLines[i] = "RunMephistoScript=" + CharConfig.RunMephistoScript;
                    if (Splitted[0] == "RunAndarielScript") AllLines[i] = "RunAndarielScript=" + CharConfig.RunAndarielScript;
                    if (Splitted[0] == "RunCountessScript") AllLines[i] = "RunCountessScript=" + CharConfig.RunCountessScript;
                    if (Splitted[0] == "RunChaosScript") AllLines[i] = "RunChaosScript=" + CharConfig.RunChaosScript;
                    if (Splitted[0] == "RunLowerKurastScript") AllLines[i] = "RunLowerKurastScript=" + CharConfig.RunLowerKurastScript;
                    if (Splitted[0] == "RunBaalLeechScript") AllLines[i] = "RunBaalLeechScript=" + CharConfig.RunBaalLeechScript;
                    if (Splitted[0] == "RunItemGrabScriptOnly") AllLines[i] = "RunItemGrabScriptOnly=" + CharConfig.RunItemGrabScriptOnly;

                    if (Splitted[0] == "RunChaosSearchGameScript") AllLines[i] = "RunChaosSearchGameScript=" + CharConfig.RunChaosSearchGameScript;
                    if (Splitted[0] == "RunBaalSearchGameScript") AllLines[i] = "RunBaalSearchGameScript=" + CharConfig.RunBaalSearchGameScript;
                    if (Splitted[0] == "RunGameMakerScript") AllLines[i] = "RunGameMakerScript=" + CharConfig.RunGameMakerScript;
                    if (Splitted[0] == "GameName") AllLines[i] = "GameName=" + CharConfig.GameName;
                    if (Splitted[0] == "GameDifficulty") AllLines[i] = "GameDifficulty=" + CharConfig.GameDifficulty;
                    if (Splitted[0] == "GamePass") AllLines[i] = "GamePass=" + CharConfig.GamePass;
                }
            }

            File.Create(File_BotSettings).Dispose();
            File.WriteAllLines(File_BotSettings, AllLines);
        }

        public void SaveOthersSettings()
        {
            string SaveTxtt = "";
            SaveTxtt += "RunNumber=" + Form1_0.CurrentGameNumber + Environment.NewLine;
            SaveTxtt += "D2_LOD_113C_Path=" + Form1_0.D2_LOD_113C_Path + Environment.NewLine;

            File.Create(File_Settings).Dispose();
            File.WriteAllText(File_Settings, SaveTxtt);
        }

        public void LoadItemsSettings()
        {
            try
            {
                bool DoingUnique = false;
                bool DoingKeysRune = false;
                bool DoingSet = false;
                bool DoingNormal = false;

                List<string> AllUnique = new List<string>();
                List<string> AllKeys = new List<string>();
                List<string> AllSet = new List<string>();
                List<string> AllNormal = new List<string>();

                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            string ThisItem = AllLines[i];
                            if (ThisItem.Contains("/"))
                            {
                                ThisItem = ThisItem.Substring(0, ThisItem.IndexOf('/'));    //remove description '//'
                            }

                            if (DoingUnique) AllUnique.Add(ThisItem);
                            if (DoingKeysRune) AllKeys.Add(ThisItem);
                            if (DoingSet) AllSet.Add(ThisItem);
                            if (DoingNormal) AllNormal.Add(ThisItem);
                        }

                        if (AllLines[i].Contains("UNIQUE ITEMS"))
                        {
                            DoingUnique = true;
                            DoingKeysRune = false;
                            DoingSet = false;
                            DoingNormal = false;
                        }
                        if (AllLines[i].Contains("KEYS/GEMS/RUNES ITEMS"))
                        {
                            DoingUnique = false;
                            DoingKeysRune = true;
                            DoingSet = false;
                            DoingNormal = false;
                        }
                        if (AllLines[i].Contains("SET ITEMS"))
                        {
                            DoingUnique = false;
                            DoingKeysRune = false;
                            DoingSet = true;
                            DoingNormal = false;
                        }
                        if (AllLines[i].Contains("NORMAL ITEMS"))
                        {
                            DoingUnique = false;
                            DoingKeysRune = false;
                            DoingSet = false;
                            DoingNormal = true;
                        }
                    }
                }

                Form1_0.ItemsAlert_0.PickItemsUnique = new string[AllUnique.Count];
                for (int i = 0; i < AllUnique.Count; i++) Form1_0.ItemsAlert_0.PickItemsUnique[i] = AllUnique[i];

                Form1_0.ItemsAlert_0.PickItemsRunesKeyGems = new string[AllKeys.Count];
                for (int i = 0; i < AllKeys.Count; i++) Form1_0.ItemsAlert_0.PickItemsRunesKeyGems[i] = AllKeys[i];

                Form1_0.ItemsAlert_0.PickItemsSet = new string[AllSet.Count];
                for (int i = 0; i < AllSet.Count; i++) Form1_0.ItemsAlert_0.PickItemsSet[i] = AllSet[i];

                //Form1_0.ItemsAlert_0.PickItemsUnique = new string[AllNormal.Count];
                //for (int i = 0; i < AllNormal.Count; i++) Form1_0.ItemsAlert_0.PickItemsUnique[i] = AllNormal[i];
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD 'ItemsSettings.txt' FILE!", Color.Red);
            }
        }

        public void LoadBotSettings()
        {
            try
            {
                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            if (AllLines[i].Contains("="))
                            {
                                string[] Params = AllLines[i].Split('=');

                                if (Params[0].Contains("MaxGameTime"))
                                {
                                    CharConfig.MaxGameTime = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("IsRushing"))
                                {
                                    CharConfig.IsRushing = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RushLeecherName"))
                                {
                                    CharConfig.RushLeecherName = Params[1];
                                }



                                if (Params[0].Contains("RunDarkWoodRush"))
                                {
                                    CharConfig.RunDarkWoodRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunTristramRush"))
                                {
                                    CharConfig.RunTristramRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunAndarielRush"))
                                {
                                    CharConfig.RunAndarielRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunRadamentRush"))
                                {
                                    CharConfig.RunRadamentRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunHallOfDeadRush"))
                                {
                                    CharConfig.RunHallOfDeadRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunFarOasisRush"))
                                {
                                    CharConfig.RunFarOasisRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunLostCityRush"))
                                {
                                    CharConfig.RunLostCityRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunSummonerRush"))
                                {
                                    CharConfig.RunSummonerRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunDurielRush"))
                                {
                                    CharConfig.RunDurielRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunKahlimEyeRush"))
                                {
                                    CharConfig.RunKahlimEyeRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunKahlimBrainRush"))
                                {
                                    CharConfig.RunKahlimBrainRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunKahlimHeartRush"))
                                {
                                    CharConfig.RunKahlimHeartRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunTravincalRush"))
                                {
                                    CharConfig.RunTravincalRush = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunMephistoRush"))
                                {
                                    CharConfig.RunMephistoRush = bool.Parse(Params[1].ToLower());
                                }

                                if (Params[0].Contains("RunPindleskinScript"))
                                {
                                    CharConfig.RunPindleskinScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunDurielScript"))
                                {
                                    CharConfig.RunDurielScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunSummonerScript"))
                                {
                                    CharConfig.RunSummonerScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunMephistoScript"))
                                {
                                    CharConfig.RunMephistoScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunAndarielScript"))
                                {
                                    CharConfig.RunAndarielScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunChaosScript"))
                                {
                                    CharConfig.RunChaosScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunCountessScript"))
                                {
                                    CharConfig.RunCountessScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunLowerKurastScript"))
                                {
                                    CharConfig.RunLowerKurastScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunBaalLeechScript"))
                                {
                                    CharConfig.RunBaalLeechScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunItemGrabScriptOnly"))
                                {
                                    CharConfig.RunItemGrabScriptOnly = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunChaosSearchGameScript"))
                                {
                                    CharConfig.RunChaosSearchGameScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunBaalSearchGameScript"))
                                {
                                    CharConfig.RunBaalSearchGameScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("RunGameMakerScript"))
                                {
                                    CharConfig.RunGameMakerScript = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("GameName"))
                                {
                                    CharConfig.GameName = Params[1];
                                }
                                if (Params[0].Contains("GamePass"))
                                {
                                    CharConfig.GamePass = Params[1];
                                }
                                if (Params[0].Contains("GameDifficulty"))
                                {
                                    CharConfig.GameDifficulty = int.Parse(Params[1]);
                                }
                                //#####
                                if (Params[0].Contains("StartStopKey"))
                                {
                                    CharConfig.StartStopKey = int.Parse(Params[1]);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD 'BotSettings.txt' FILE!", Color.Red);
            }
        }

        public void LoadCubingSettings()
        {
            try
            {
                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            if (AllLines[i].Contains("="))
                            {
                                Form1_0.Cubing_0.CubingRecipes.Add(AllLines[i]);
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD 'CubingRecipes.txt' FILE!", Color.Red);
            }
        }

        public void LoadCharSettings()
        {
            try
            {
                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            if (AllLines[i].Contains("="))
                            {
                                string[] Params = AllLines[i].Split('=');

                                if (Params[0].Contains("RunOnChar"))
                                {
                                    CharConfig.RunningOnChar = Params[1];
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD 'CharSettings.txt' FILE!", Color.Red);
            }
        }

        public void LoadCurrentCharSettings()
        {
            try
            {
                for (int i = 0; i < AllLines.Length; i++)
                {
                    if (AllLines[i].Length > 0)
                    {
                        if (AllLines[i][0] != '/' && AllLines[i][0] != '#')
                        {
                            if (AllLines[i].Contains("="))
                            {
                                string[] Params = AllLines[i].Split('=');

                                if (Params[0].Contains("KeySkillAttack"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillAttack);
                                }
                                if (Params[0].Contains("KeySkillAura"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillAura);
                                }
                                if (Params[0].Contains("KeySkillfastMoveAtTown"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillfastMoveAtTown);
                                }
                                if (Params[0].Contains("KeySkillfastMoveOutsideTown"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillfastMoveOutsideTown);
                                }
                                if (Params[0].Contains("KeySkillDefenseAura"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillDefenseAura);
                                }
                                if (Params[0].Contains("KeySkillCastDefense"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillCastDefense);
                                }
                                if (Params[0].Contains("KeySkillLifeAura"))
                                {
                                    Enum.TryParse(Params[1], out CharConfig.KeySkillLifeAura);
                                }
                                //######
                                if (Params[0].Contains("BeltPotTypeToHave") && Params[1].Contains(","))
                                {
                                    string[] NewParams = Params[1].Split(',');
                                    if (NewParams.Length >= 4)
                                    {
                                        CharConfig.BeltPotTypeToHave = new int[4] { int.Parse(NewParams[0]),
                                                                                int.Parse(NewParams[1]),
                                                                                int.Parse(NewParams[2]),
                                                                                int.Parse(NewParams[3]) };
                                    }
                                }
                                if (Params[0].Contains("InventoryDontCheckItem"))
                                {
                                    string[] NewParams1 = AllLines[i + 2].Split(',');
                                    string[] NewParams2 = AllLines[i + 3].Split(',');
                                    string[] NewParams3 = AllLines[i + 4].Split(',');
                                    string[] NewParams4 = AllLines[i + 5].Split(',');

                                    if (NewParams1.Length >= 10 && NewParams2.Length >= 10 && NewParams3.Length >= 10 && NewParams4.Length >= 10)
                                    {
                                        CharConfig.InventoryDontCheckItem = new int[40];
                                        for (int k = 0; k < 10; k++)
                                        {
                                            CharConfig.InventoryDontCheckItem[k] = int.Parse(NewParams1[k]);
                                        }
                                        for (int k = 0; k < 10; k++)
                                        {
                                            CharConfig.InventoryDontCheckItem[k + 10] = int.Parse(NewParams2[k]);
                                        }
                                        for (int k = 0; k < 10; k++)
                                        {
                                            CharConfig.InventoryDontCheckItem[k + 20] = int.Parse(NewParams3[k]);
                                        }
                                        for (int k = 0; k < 10; k++)
                                        {
                                            CharConfig.InventoryDontCheckItem[k + 30] = int.Parse(NewParams4[k]);
                                        }
                                    }
                                }
                                //#####
                                if (Params[0].Contains("DummyItemSharedStash1"))
                                {
                                    CharConfig.DummyItemSharedStash1 = Params[1];
                                }
                                if (Params[0].Contains("DummyItemSharedStash2"))
                                {
                                    CharConfig.DummyItemSharedStash2 = Params[1];
                                }
                                if (Params[0].Contains("DummyItemSharedStash3"))
                                {
                                    CharConfig.DummyItemSharedStash3 = Params[1];
                                }
                                //#####
                                if (Params[0].Contains("PlayerCharName"))
                                {
                                    CharConfig.PlayerCharName = Params[1];
                                }
                                if (Params[0].Contains("UseTeleport"))
                                {
                                    CharConfig.UseTeleport = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("ChickenHP"))
                                {
                                    CharConfig.ChickenHP = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("TakeHPPotUnder") && !Params[0].Contains("MercTakeHPPotUnder"))
                                {
                                    CharConfig.TakeHPPotUnder = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("TakeRVPotUnder"))
                                {
                                    CharConfig.TakeRVPotUnder = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("TakeManaPotUnder"))
                                {
                                    CharConfig.TakeManaPotUnder = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("GambleAboveGoldAmount"))
                                {
                                    CharConfig.GambleAboveGoldAmount = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("GambleUntilGoldAmount"))
                                {
                                    CharConfig.GambleUntilGoldAmount = int.Parse(Params[1]);
                                }
                                if (Params[0].Contains("PlayerAttackWithRightHand"))
                                {
                                    CharConfig.PlayerAttackWithRightHand = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("KeysLocationInInventory"))
                                {
                                    //8,0
                                    string KeyValue1 = Params[1].ToLower().Substring(0, Params[1].IndexOf(","));
                                    string KeyValue2 = Params[1].ToLower().Substring(Params[1].IndexOf(",") + 1);
                                    CharConfig.KeysLocationInInventory.Item1 = int.Parse(KeyValue1);
                                    CharConfig.KeysLocationInInventory.Item2 = int.Parse(KeyValue2);
                                }
                                //#####
                                if (Params[0].Contains("UsingMerc"))
                                {
                                    CharConfig.UsingMerc = bool.Parse(Params[1].ToLower());
                                }
                                if (Params[0].Contains("MercTakeHPPotUnder"))
                                {
                                    CharConfig.MercTakeHPPotUnder = int.Parse(Params[1]);
                                }
                                //#####

                                //#####
                            }
                        }
                    }
                }
            }
            catch
            {
                Form1_0.method_1("UNABLE TO LOAD THE SETTINGS FILE FOR CURRENT CHAR!", Color.Red);
            }
        }
    }
}