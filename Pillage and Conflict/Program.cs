using System;

namespace Pillage_and_Conflict
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (var game = new PillageandConflict())
                game.Run();
        }
    }
}