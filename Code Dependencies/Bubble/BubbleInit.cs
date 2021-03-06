// Lic:
// NALA
// Init
// 
// 
// 
// (c) Jeroen P. Broks, 2019
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
// Version: 20.07.19
// EndLic



using System;
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
            SBubble.AddInit(Bubble_Conf.Init);
            SBubble.AddInit(Bubble_GameJolt.Init);
            SBubble.AddInit(BubbleTimer.Init);
            SBubble.AddInit(Bubble_Swap.Init);            
            #endregion

            #region Extras to use in savegames
            Bubble_Save.SaveXtra["RPGPARTY"] = delegate (UseJCR6.TJCRCreate j, string dir) {
                RPG.RPGSave(j, $"XTRA/{dir}");
            };
            Bubble_Save.LoadXtra["RPGPARTY"] = delegate (UseJCR6.TJCRDIR j, string dir) {
                RPG.RPGLoad(j, $"XTRA/{dir}");
                return true;
            };
            Bubble_Save.SaveXtra["SWAP"] = Bubble_Swap.SwapSave;
            Bubble_Save.LoadXtra["SWAP"] = Bubble_Swap.SwapLoad;
            #endregion

            #region The stuff needed for NALA in particular
            // Own APIs for NIL and Lua
            //SBubble.AddInit(MapScriptAPI.API_Init);
            SBubble.AddInit(RPG_API.Init);

            // Kthura Init
            Kthura.SetDefaultTextureJCR(SBubble.JCR); // Textures are all in the same project
            KthuraDraw.DrawDriver = new KthuraDrawMonoGame(); // Tell Kthura to use the MonoGame Driver to display the map
            KthuraDrawMonoGame.CrashOnNoTex = delegate (string msg) { SBubble.MyError("Kthura Texture Error", msg, ""); };

            // SBubble Kthura API
            SBubble.AddInit(KthuraBubble.KthuraBubble.Init);
            SBubble.AddInit(KthuraBubble.KthBlockAPI.Init);
            SBubble.AddInit(KthuraBubble.KthuraAbyssGenerator.Init);

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