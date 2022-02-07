using System;

namespace ELEPHANTSRPG
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ELEPHANTS())
                game.Run();
        }
    }
}
