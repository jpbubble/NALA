using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bubble;
using TrickyUnits;

namespace NALA.Code_Dependencies.NALA {
    class RPG_API {
        static RPG_API me;
        string statename;
        static void Init(string state) {
            me = new RPG_API();
            var script = QuickStream.StringFromEmbed("RPG_API.nil");
            SBubble.State(state).state["NALA_RPG"] = me;
            SBubble.DoNIL(state, script, "Link code RPG API");
            me.statename = state;
        }

        #region Party Members
        public void setPartyMember(int index, string tag) {
            if (index - 1 >= RPG.RPGParty.Length || index <= 0) {
                SBubble.MyError("RPG API Error", "Party Member index out of bounds", SBubble.TraceLua(statename));
                return;
            }
            RPG.RPGParty[index - 1] = tag;
        }

        public string getPartyMember(int index) {
            if (index - 1 >= RPG.RPGParty.Length || index <= 0) {
                SBubble.MyError("RPG API Error", "Party Member index out of bounds", SBubble.TraceLua(statename));
                return "";
            }
            return RPG.RPGParty[index - 1];
        }

        public int PartyMax {
            set => RPG.RPGParty = new string[value];
            get => RPG.RPGParty.Length;
        }
        #endregion

        #region Character creation, management and destruction
        public void CreateChar(string tag) { new RPGCharacter(tag); }
        public bool CharExists(string tag) => RPG.RPGChars.Contains(tag);
        #endregion

        #region Stats
        public bool StatExists(string chrtag, string stattag) {
            if (!CharExists(chrtag)) return false;
            return RPG.GrabChar(chrtag).HasStat(stattag);
        }

        public void CreateStat(string chrtag, string statag, bool overwrite) {
            if (!CharExists(chrtag)) {
                SBubble.MyError($"CreateStat(\"{chrtag}\",\"{statag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename));
                return;
            }
            var ch = RPG.GrabChar(chrtag);
            ch.CreateStat(statag, overwrite);
        }

        public string StatScript(string chrtag, string statag, string script) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) SBubble.MyError($"Script Stat(\"{chrtag}\",\"{statag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename));
            var st = ch.Stat(statag); if (st == null) SBubble.MyError($"Script Stat(\"{chrtag}\",\"{statag}\"):", "Stat doesn't exist!", SBubble.TraceLua(statename));
            if (script == "**DONOTCHANGE**") return st.ScriptFile; // Pleaase note that in NALA I'm gonna go farther than just script files, as they sometimes make things more complicated than needed
            st.ScriptFile = script;
            return script;
        }

        public int GetStatValue(string chrtag, string stattag) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) SBubble.MyError($"GetStatValue(\"{chrtag}\",\"{stattag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename));
            var st = ch.Stat(stattag); if (st == null) SBubble.MyError($"GetStatValue(\"{chrtag}\",\"{stattag}\"):", "Stat doesn't exist!", SBubble.TraceLua(statename));
            if (st.ScriptFile == "") return st.Value;
            var DubbelePunt = st.ScriptFile.IndexOf(':');
            string Kind, Formula;
            if (DubbelePunt >= 0) {
                Kind = st.ScriptFile.Substring(0, DubbelePunt).ToUpper();
                Formula = st.ScriptFile.Substring(DubbelePunt + 1);
            } else {
                Kind = "FILE";
                Formula = st.ScriptFile;
            }
            switch (Kind) {
                case "FILE":
                    // TODO: Implement FILE: prefix for GetStatValue scripting!
                    SBubble.MyError("Too early version error", "FILE: not yet supported in NALA", "");
                    return 0;
                case "SUM":
                case "AVG": {
                        var Stats = Formula.Split(',');
                        var Total = 0;
                        foreach (string calcStat in Stats) Total = +GetStatValue(chrtag, calcStat);
                        switch (Kind) {
                            case "SUM": st.Value = Total; break;
                            case "AVG": st.Value = (int)Math.Floor((decimal)Total / (decimal)Stats.Length); break;
                        }
                    }
                    return st.Value;
                default:
                    SBubble.MyError("Stat Script Error", $"Script kind {Kind} unknown", $"Char: {chrtag}; Stat {stattag}");
                    return 0;
            }
        }

        public void SetStatValue(string chrtag, string stattag, int value) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) SBubble.MyError($"GetStatValue(\"{chrtag}\",\"{stattag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename));
            var st = ch.Stat(stattag); if (st == null) SBubble.MyError($"GetStatValue(\"{chrtag}\",\"{stattag}\"):", "Stat doesn't exist!", SBubble.TraceLua(statename));
            if (st.ScriptFile == "") {
                SBubble.MyError($"SetStatValue(\"{chrtag}\", \"{stattag}\", {value});", "Scripted stats cannot have their value manually reassinged!", SBubble.TraceLua(statename));
                return;
            }
            st.Value = value;
        }
        #endregion

        #region Points
        public void ChkPoints(string chrtag, string pnttag, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename));
            ch.Point(pnttag, createifneeded);
        }

        public int GetMaxPoints(string chrtag, string pnttag, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            if (pt.MaxCopy != "") pt.Maximum = GetStatValue(chrtag, pt.MaxCopy);
            return pt.Maximum;
        }

        public void SetMaxPoints(string chrtag, string pnttag, int value, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return; }
            if (pt.MaxCopy != "") {
                SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "MaxCopy based points cannot have their maximum value manually altered", SBubble.TraceLua(statename));
                return;
            }
            if (pt.Minimum > pt.Maximum) {
                SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Minimum higher than maximum", SBubble.TraceLua(statename));
                return;
            }
            pt.Maximum = value;
        }


        public int GetMinPoints(string chrtag, string pnttag, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            return pt.Minimum;
        }

        public void SetMinPoints(string chrtag, string pnttag, int value, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return; }
            if (pt.Minimum > pt.Maximum) {
                SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Minimum higher than maximum", SBubble.TraceLua(statename));
                return;
            }
            pt.Minimum = value;
        }


        public int GetHavePoints(string chrtag, string pnttag, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            if (pt.Have < pt.Minimum) pt.Have = pt.Minimum;
            if (pt.Have > pt.Maximum) pt.Have = pt.Maximum;
            return pt.Have;
        }

        public void SetHavePoints(string chrtag, string pnttag, int value, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return; }
            pt.Have = value;
            if (pt.Have < pt.Minimum) pt.Have = pt.Minimum;
            if (pt.Have > pt.Maximum) pt.Have = pt.Maximum;
        }

        public string GetMaxCopyPoints(string chrtag, string pnttag, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return 0; }
            return pt.MaxCopy;
        }

        public void SetMaxCopyPoints(string chrtag, string pnttag, string value, bool createifneeded) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return; }
            var pt = ch.Point(pnttag, createifneeded); if (pt == null) { SBubble.MyError($"ChkPoints(\"{chrtag}\",\"{pnttag}\"):", "Points record doesn't exist!", SBubble.TraceLua(statename)); return; }
            pt.MaxCopy = value;
            pt.Maximum = GetStatValue(chrtag, pt.MaxCopy);
        }

        #endregion
        #region Name
        public void SetName(string chrtag, string Name) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"Name(\"{chrtag}\",\"{Name}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return; }
            ch.Name = Name;
        }

        public string GetName(string chrtag) {
            var ch = RPG.GrabChar(chrtag); if (ch == null) { SBubble.MyError($"Name(\"{chrtag}\"):", "Character doesn't exist!", SBubble.TraceLua(statename)); return "Heroninumus Petrus Birokasu"; }
            return ch.Name;
        }
        #endregion

    }
}

}
