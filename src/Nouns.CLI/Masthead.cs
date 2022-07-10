using System;

namespace Nouns.CLI
{
    internal static class Masthead
    {
        public static void Print()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@" ____    ___   __ __  ____   _____  ____   ____  ___ ___    ___     __    __  ______  _____ 
|    \  /   \ |  |  ||    \ / ___/ /    | /    ||   |   |  /  _]   |  |__|  ||      ||     |
|  _  ||     ||  |  ||  _  (   \_ |   __||  o  || _   _ | /  [_    |  |  |  ||      ||   __|
|  |  ||  O  ||  |  ||  |  |\__  ||  |  ||     ||  \_/  ||    _]   |  |  |  ||_|  |_||  |_  
|  |  ||     ||  :  ||  |  |/  \ ||  |_ ||  _  ||   |   ||   [_  __|  `  '  |  |  |  |   _] 
|  |  ||     ||     ||  |  |\    ||     ||  |  ||   |   ||     ||  |\      /   |  |  |  |   
|__|__| \___/  \__,_||__|__| \___||___,_||__|__||___|___||_____||__| \_/\_/    |__|  |__|");
            Console.ForegroundColor = color;
            Console.WriteLine();
        }
    }
}
