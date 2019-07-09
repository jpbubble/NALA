using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrickyUnits;
using Bubble;

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
            // TODO: API for Kthura
            // TODO: API for TeddyBear
            // TODO: API for Swap Data Manager
            // TODO: API for RPGStat
            #endregion

            // Start init script
            FlowManager.StartInitFlow();
        }
    }
}
