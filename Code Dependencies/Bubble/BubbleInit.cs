// Lic:
// NALA
// Init
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
// Version: 19.07.09
// EndLic

ï»¿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyUnits;
using Bubble;
using NSKthura;

namespace NALA {
    static class BubbleInit {
        public static void LetsGo() {

            #region Install Error Manager, and the global engine!
            SBubble.Init("NALA", Error.GoError);
            NALACore.Core.Window.Title = SBubble.Title;
            #endregion


            #region The base MonoGame stuff
            SBubble.AddInit(delegate (string v) { SBubble.State(v).DoString($"function {FlowManager.NOTHING}() end", "Alright move along, nothing to see here!"); });
            SBubble.AddInit(BubbleConsole.StateInit);
            SBubble.AddInit(BubbleGraphics.InitGraphics);
            SBubble.AddInit(Bubble_Audio.Init);
            SBubble.AddInit(APIFlow.Init);
            SBubble.AddInit(BubbleSuperGlobal.Init);
            SBubble.AddInit(Bubble_Input.Init);
            SBubble.AddInit(Bubble_Save.Init);
            #endregion

            #region The stuff needed for NALA in particular
            // Kthura Init
            Kthura.SetDefaultTextureJCR(SBubble.JCR); // Textures are all in the same project
            KthuraDraw.DrawDriver = new KthuraDrawMonoGame(); // Tell Kthura to use the MonoGame Driver to display the map
            // SBubble Kthura API

            // TODO: API for TeddyBear
            // TODO: API for Swap Data Manager
            // TODO: API for RPGStat
            RPG.JCRSTORAGE = "lzma";
            #endregion

            // Start init script
            FlowManager.StartInitFlow();
        }
    }
}

