// Lic:
// MapScriptAPI - NALA
// .
// 
// 
// 
// (c) Jeroen P. Broks, 
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Please note that some references to data like pictures or audio, do not automatically
// fall under this licenses. Mostly this is noted in the respective files.
// 
// Version: 19.07.12
// EndLic


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


