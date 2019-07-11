using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyUnits;
using Bubble;

namespace NALA {
    class MapScriptAPI {

        private MapScriptAPI() { }
        
        static public void API_Init(string namestate) {
            var ret = new MapScriptAPI();
            var state = SBubble.State(namestate).state;
            var script = QuickStream.StringFromEmbed("MapScriptAPI.nil");
            state["NALAMSAPI"] = ret;
            SBubble.DoNIL(namestate, script, "MapScript init script");
        }

    }
}
