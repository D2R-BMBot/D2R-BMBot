using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MapAreaStruc;

    public interface IBot
    {
        string ScriptName { get; }
        string ScriptType { get; }
        int CurrentStep { get; set; }
        bool ScriptDone { get; set; }

    void SetForm1(Form1 form);
        void ResetVars();
        void RunScript();
    }

    public interface IRushBot : IBot
    {
        string ScriptAct { get; }
        string ScriptQuest { get; }
    }

    public static class ScriptHelper
    {

        public static void UpdateLocation(Form1 form)
        {
            form.PlayerScan_0.UpdateLocation();
        }
    }
