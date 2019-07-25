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

        #region Character creation and destruction
        public void CreateChar(string tag) { new RPGCharacter(tag); }
        public bool CharExists(string tag) => RPG.RPGChars.Contains(tag);
        #endregion

        #region Stats
        public bool StatExists(string chrtag,string stattag) {
            if (!CharExists(chrtag)) return false;
            return RPG.GrabChar(chrtag).HasStat(stattag);
        }

        public int DStat(string tag,string stat,int value) {
            try {
                var ch = RPG.GrabChar(tag); if (ch == null) throw new Exception($"No character named {tag}");
                ch.CreateStat(stat, false);
                var st = ch.Stat(stat);
                st.Value = value;
                return st.Value;
            } catch (Exception noway) {
                SBubble.MyError("RPG Stat Error", noway.Message, SBubble.TraceLua(statename));
            return value;
            }
        }
        public int GStat(string tag, string stat) {
            try {
                var ch = RPG.GrabChar(tag); if (ch == null) throw new Exception($"No character named {tag}");                
                var st = ch.Stat(stat); if (st == null) throw new Exception($"Character {tag} doesn't have the stat {stat}");
                return st.Value;
            } catch (Exception noway) {
                SBubble.MyError("RPG Stat Error", noway.Message, SBubble.TraceLua(statename));
                return 0;
            }            
        }
        #endregion
    }

}
