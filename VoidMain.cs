using System;

namespace NALA
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class VoidMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new NALACore())
                game.Run();
        }
    }
}
