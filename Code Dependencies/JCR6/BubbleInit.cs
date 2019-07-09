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

