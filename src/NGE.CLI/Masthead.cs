using System;

namespace NGE.CLI
{
    internal static class Masthead
    {
        public static void Print()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@" ____    ____    ___ 
|    \  /    |  /  _]
|  _  ||   __| /  [_ 
|  |  ||  |  ||    _]
|  |  ||  |_ ||   [_ 
|  |  ||     ||     |
|__|__||___,_||_____|");
            Console.ForegroundColor = color;
            Console.WriteLine();
        }
    }
}
