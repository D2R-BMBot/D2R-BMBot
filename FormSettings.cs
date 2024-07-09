using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public partial class FormSettings : Form
{
    Form1 Form1_0;
    public BotLoader botLoader;

    public FormSettings(Form1 form1_1)
    {
        Form1_0 = form1_1;
        botLoader = BotLoader.GetInstance(Form1_0);
        InitializeComponent();
        this.TopMost = true;

        ConfigureComboBoxScriptType();
        PopulateKeyCombos();
        InitializeUIComponents();

        botLoader.LoadScripts();
        PopulateListViews();
    }
    private void PopulateKeyCombos()
    {
        textBoxStartKey.Items.Clear();
        comboBoxPauseResume.Items.Clear();
        string[] names = Enum.GetNames(typeof(Keys));
        foreach (var name in names)
        {
            textBoxStartKey.Items.Add(name);
            comboBoxPauseResume.Items.Add(name);
        }
    }
    private void ConfigureComboBoxScriptType()
    {
        comboBoxScriptType.Items.Add("Bots");
        comboBoxScriptType.Items.Add("Leech");
        comboBoxScriptType.Items.Add("Rush");

        comboBoxScriptType.SelectedIndexChanged += ComboBoxScriptType_SelectedIndexChanged;
        comboBoxScriptType.SelectedIndex = 0; // Set default selection to "Bots"
    }

    private void HideAllPanels()
    {
        groupBoxSearch.Visible = false;
        groupBoxSearch.Location = new Point(groupBox1.Location.X, groupBox1.Location.Y);

        listViewRush.Visible = false;
        listViewRush.Location = new Point(listViewRunScripts.Location.X, listViewRunScripts.Location.Y);

        panelBaalFeature.Visible = false;
        panelBaalFeature.Location = new Point(23, 197);

        panelOverlay.Visible = false;
        panelOverlay.Location = new Point(23, 197);

        panelChaosFeature.Visible = false;
        panelChaosFeature.Location = new Point(23, 197);

        panelBaalLeech.Visible = false;
        panelBaalLeech.Location = new Point(23, 197);

        panelShopBot.Visible = false;
        panelShopBot.Location = new Point(23, 197);
    }
    private void InitializeUIComponents()
    {
        // Additional UI initialization code
        groupBoxSearch.Visible = false;
        groupBoxSearch.Location = new Point(groupBox1.Location.X, groupBox1.Location.Y);

        listViewRush.Visible = false;
        listViewRush.Location = new Point(listViewRunScripts.Location.X, listViewRunScripts.Location.Y);

        panelBaalFeature.Visible = false;
        panelBaalFeature.Location = new Point(23, 197);

        panelOverlay.Visible = false;
        panelOverlay.Location = new Point(23, 197);

        panelChaosFeature.Visible = false;
        panelChaosFeature.Location = new Point(23, 197);

        panelBaalLeech.Visible = false;
        panelBaalLeech.Location = new Point(23, 197);

        panelShopBot.Visible = false;
        panelShopBot.Location = new Point(23, 197);

        listViewRunScripts.ItemChecked += listView_ItemChecked;
        listViewRush.ItemChecked += listView_ItemChecked;
    }

    private void ComboBoxScriptType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FilterAndDisplayScripts();
    }

    private void FilterAndDisplayScripts()
    {
        if (comboBoxScriptType.SelectedItem == null)
        {
            listViewRunScripts.Items.Clear();
            listViewRush.Items.Clear();
            return;
        }

        string selectedType = comboBoxScriptType.SelectedItem.ToString();
        List<IBot> filteredScripts;

        switch (selectedType)
        {
            case "Bots":
                filteredScripts = botLoader.Scripts.Where(s => s.ScriptType == "Bot").ToList();
                PopulateListView(listViewRunScripts, filteredScripts);
                listViewRunScripts.Visible = true;
                listViewRush.Visible = false;
                break;
            case "Leech":
                filteredScripts = botLoader.Scripts.Where(s => s.ScriptType == "Leech").ToList();
                PopulateListView(listViewRunScripts, filteredScripts);
                listViewRunScripts.Visible = true;
                listViewRush.Visible = false;
                break;
            case "Rush":
                filteredScripts = botLoader.Scripts.OfType<IRushBot>()
                    .OrderBy(s => s.ScriptAct)
                    .ThenBy(s => s.ScriptQuest, new AlphanumericComparer())
                    .ToList<IBot>();
                PopulateListView(listViewRush, filteredScripts);
                listViewRunScripts.Visible = false;
                listViewRush.Visible = true;
                break;
        }
    }

    private void PopulateListViews()
    {
        try
        {
            FilterAndDisplayScripts();
        }
        catch (Exception ex)
        {
            Form1_0.method_1($"Error loading scripts: {ex.Message}", Color.Red);
        }
    }

    private void PopulateListView(ListView listView, List<IBot> scripts)
    {
        listView.Items.Clear(); // Clear existing items before adding new ones
        foreach (var script in scripts)
        {
            ListViewItem item = new ListViewItem(script.ScriptName)
            {
                Tag = botLoader.GetFileName(script),
                Checked = botLoader.ScriptCheckedStates.TryGetValue(botLoader.GetFileName(script), out bool isChecked) && isChecked
            };
            listView.Items.Add(item);
        }
    }
    private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
        if (e.Item.Tag is string scriptName)
        {
            botLoader.ScriptCheckedStates[scriptName] = e.Item.Checked;
        }
    }

    private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Implement any necessary cleanup here
    }

    private void comboBoxLobby_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetCreateGameGroupbox();
    }

    private void SetCreateGameGroupbox()
    {
        if (!checkBoxRush.Checked)
        {
            groupBox1.Enabled = (comboBoxLobby.SelectedIndex == 0);
        }
        else
        {
            groupBox1.Enabled = false;
        }

        if (comboBoxLobby.SelectedIndex == 1)
        {
            textBoxSearchGame.Text = ""; // Set according to your logic
            textBoxAvoidWords.Text = ""; // Set according to your logic
        }
        else if (comboBoxLobby.SelectedIndex == 2)
        {
            textBoxSearchGame.Text = ""; // Set according to your logic
            textBoxAvoidWords.Text = ""; // Set according to your logic
        }

        if (comboBoxLobby.SelectedIndex == 1 || comboBoxLobby.SelectedIndex == 2)
        {
            groupBox1.Visible = false;
            groupBoxSearch.Visible = true;
            textBox2LeechName.Text = ""; // Set according to your logic
        }
        else
        {
            groupBox1.Visible = true;
            groupBoxSearch.Visible = false;
        }

        if (comboBoxLobby.SelectedIndex == 4)
        {
            groupBox1.Visible = true;
            groupBoxSearch.Visible = false;
        }
    }
    public void SaveSettings()
    {
        CharConfig.ShowOverlay = checkBoxShowOverlay.Checked;

        Form1_0.D2_LOD_113C_Path = textBoxD2Path.Text;
        CharConfig.MaxGameTime = (int)numericUpDownMaxTime.Value;
        CharConfig.IsRushing = checkBoxRush.Checked;
        CharConfig.RushLeecherName = textBox1LeechName.Text;
        Enum.TryParse(textBoxStartKey.Text, out CharConfig.StartStopKey);
        Enum.TryParse(comboBoxPauseResume.Text, out CharConfig.PauseResumeKey);

        CharConfig.LogNotUsefulErrors = checkBoxLogOrangeError.Checked;

        // Dynamically save script settings from botLoader.ScriptCheckedStates
        foreach (var kvp in botLoader.ScriptCheckedStates)
        {
            CharConfig.ScriptStates[kvp.Key] = kvp.Value;
        }

        CharConfig.RunGameMakerScript = (comboBoxLobby.SelectedIndex == 0);
        CharConfig.RunChaosSearchGameScript = (comboBoxLobby.SelectedIndex == 1);
        CharConfig.RunBaalSearchGameScript = (comboBoxLobby.SelectedIndex == 2);
        CharConfig.RunNoLobbyScript = (comboBoxLobby.SelectedIndex == 3);
        CharConfig.RunSinglePlayerScript = (comboBoxLobby.SelectedIndex == 4);

        CharConfig.GameName = textBoxGameName.Text;
        CharConfig.GamePass = textBoxGamePass.Text;

        CharConfig.GameDifficulty = comboBoxDifficulty.SelectedIndex;

        Form1_0.CurrentGameNumber = (int)numericUpDownRunNumber.Value;

        ////###################
        ////SPECIALS BAAL FEATURES
        //Form1_0.Baal_0.KillBaal = checkBoxKillBaal.Checked;
        //Form1_0.Baal_0.SafeYoloStrat = checkBoxBaalSafeHealing.Checked;
        //Form1_0.Baal_0.LeaveIfMobsCountIsAbove = (int)numericUpDownBaalLeaveMobsCount.Value;
        //Form1_0.Baal_0.LeaveIfMobsIsPresent_ID.Clear();
        //Form1_0.Baal_0.LeaveIfMobsIsPresent_Count.Clear();
        //for (int i = 0; i < listViewBaalLeaveOnMobs.Items.Count; i++)
        //{
        //    Form1_0.Baal_0.LeaveIfMobsIsPresent_ID.Add(uint.Parse(listViewBaalLeaveOnMobs.Items[i].SubItems[0].Text));
        //    Form1_0.Baal_0.LeaveIfMobsIsPresent_Count.Add(int.Parse(listViewBaalLeaveOnMobs.Items[i].SubItems[1].Text));
        //}

        //Form1_0.BaalLeech_0.BaalLeechFight = checkBoxBaalLeechFightMobs.Checked;

        ////###################
        ////SPECIALS CHAOS FEATURES
        //Form1_0.Chaos_0.FastChaos = checkBoxFastChaos.Checked;

        //###################
        //SPECIALS OVERLAY FEATURES
        Form1_0.overlayForm.ShowMobs = checkBoxOverlayShowMobs.Checked;
        Form1_0.overlayForm.ShowWPs = checkBoxOverlayShowWP.Checked;
        Form1_0.overlayForm.ShowGoodChests = checkBoxOverlayShowGoodChest.Checked;
        Form1_0.overlayForm.ShowLogs = checkBoxOverlayShowLogs.Checked;
        Form1_0.overlayForm.ShowBotInfos = checkBoxOverlayShowBotInfos.Checked;
        Form1_0.overlayForm.ShowNPC = checkBoxOverlayShowNPC.Checked;
        Form1_0.overlayForm.ShowPathFinding = checkBoxOverlayShowPath.Checked;
        Form1_0.overlayForm.ShowExits = checkBoxOverlayShowExits.Checked;
        Form1_0.overlayForm.ShowMapHackShowLines = checkBoxOverlayShowMH.Checked;
        Form1_0.overlayForm.ShowUnitsScanCount = checkBoxOverlayShowUnitsCount.Checked;
        //###################

        if (comboBoxLobby.SelectedIndex == 1)
        {
            CharConfig.ChaosLeechSearch = textBoxSearchGame.Text;

            CharConfig.ChaosSearchAvoidWords.Clear();
            if (textBoxAvoidWords.Text.Contains(","))
            {
                string[] AllNames = textBoxAvoidWords.Text.Split(',');
                for (int p = 0; p < AllNames.Length; p++) CharConfig.ChaosSearchAvoidWords.Add(AllNames[p]);
            }
            else if (textBoxAvoidWords.Text != "") CharConfig.ChaosSearchAvoidWords.Add(textBoxAvoidWords.Text);
        }
        else if (comboBoxLobby.SelectedIndex == 2)
        {
            CharConfig.BaalLeechSearch = textBoxSearchGame.Text;

            CharConfig.BaalSearchAvoidWords.Clear();
            if (textBoxAvoidWords.Text.Contains(","))
            {
                string[] AllNames = textBoxAvoidWords.Text.Split(',');
                for (int p = 0; p < AllNames.Length; p++) CharConfig.BaalSearchAvoidWords.Add(AllNames[p]);
            }
            else if (textBoxAvoidWords.Text != "") CharConfig.BaalSearchAvoidWords.Add(textBoxAvoidWords.Text);
        }
        CharConfig.SearchLeecherName = textBox2LeechName.Text;

        //###################
        //SHOP BOT
        //Form1_0.ShopBot_0.MaxShopCount = int.Parse(numericUpDownMaxShopCount.Value.ToString());
        //Form1_0.ShopBot_0.ShopBotTownAct = int.Parse(numericUpDownShopTownAct.Value.ToString());
        //###################
    }

    private void listViewRunScripts_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handle ListView item selection if necessary
    }

    private void FormSettings_Load(object sender, EventArgs e)
    {
        // Handle form load event if necessary
    }

    private void button2_Click(object sender, EventArgs e)
    {
        SaveSettings();
        Form1_0.SettingsLoader_0.SaveBotSettings();
    }

    private void buttonReload_Click(object sender, EventArgs e)
    {
        DialogResult result = openFileDialog1.ShowDialog();
        if (result == DialogResult.OK)
        {
            Form1_0.SettingsLoader_0.LoadThisFileSettings(openFileDialog1.FileName);
            LoadScriptStates();
            //LoadSettings();
            Application.DoEvents();
        }
    }

    private void button3_Click(object sender, EventArgs e)
    {
        panelOverlay.Visible = false;
    }

    private void buttonOverlaySettings_Click(object sender, EventArgs e)
    {
        panelOverlay.Visible = true;
    }

    private void button4_Click(object sender, EventArgs e)
    {
        panelChaosFeature.Visible = false;
    }

    private void button5_Click(object sender, EventArgs e)
    {
        FormAdvancedSettings FormAdvancedSettings_0 = new FormAdvancedSettings(Form1_0);
        FormAdvancedSettings_0.StartPosition = FormStartPosition.Manual;
        FormAdvancedSettings_0.Location = new Point(this.Location.X + this.Width, this.Location.Y);
        FormAdvancedSettings_0.ShowDialog();
    }

    private void button6_Click(object sender, EventArgs e)
    {
        panelBaalLeech.Visible = false;
    }

    private void buttonApplyShopBot_Click(object sender, EventArgs e)
    {
        panelShopBot.Visible = false;
    }

    public void LoadScriptStates()
    {
        foreach (ListViewItem item in listViewRunScripts.Items)
        {
            string scriptName = item.Tag as string;
            if (scriptName != null && botLoader.ScriptCheckedStates.TryGetValue(scriptName, out bool isChecked))
            {
                item.Checked = isChecked;
            }
        }

        foreach (ListViewItem item in listViewRush.Items)
        {
            string scriptName = item.Tag as string;
            if (scriptName != null && botLoader.ScriptCheckedStates.TryGetValue(scriptName, out bool isChecked))
            {
                item.Checked = isChecked;
            }
        }
    }
}

public class AlphanumericComparer : IComparer<string>
{
    public int Compare(string x, string y)
    {
        if (x == y) return 0;
        if (string.IsNullOrEmpty(x)) return -1;
        if (string.IsNullOrEmpty(y)) return 1;

        int ix = 0, iy = 0;

        while (ix < x.Length && iy < y.Length)
        {
            if (char.IsDigit(x[ix]) && char.IsDigit(y[iy]))
            {
                int startX = ix, startY = iy;

                while (ix < x.Length && char.IsDigit(x[ix])) ix++;
                while (iy < y.Length && char.IsDigit(y[iy])) iy++;

                int lengthX = ix - startX, lengthY = iy - startY;

                string numX = x.Substring(startX, lengthX);
                string numY = y.Substring(startY, lengthY);

                int compareNumbers = lengthX == lengthY ? string.Compare(numX, numY, StringComparison.Ordinal) : lengthX.CompareTo(lengthY);

                if (compareNumbers != 0)
                    return compareNumbers;
            }
            else
            {
                int compareChars = x[ix].CompareTo(y[iy]);
                if (compareChars != 0)
                    return compareChars;

                ix++;
                iy++;
            }
        }

        return x.Length.CompareTo(y.Length);
    }
}
