using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Loads and initizes the JCR6 classes needed for NALA
// Please note that JCR6 itself and the drivers for lzma and zlib compression need to be properly loaded
// Not to mention the drivers for JCR5 (if you wanna use it) and RealDir (recommended for debugging purposes only)

namespace UseJCR6 {
    static class BubbleInit {

        public static void Init() {
            JCR6_lzma.Init();
            JCR6_zlib.Init();
            new JCR6_RealDir();
            new JCR_JCR5();
        }
    }
}
