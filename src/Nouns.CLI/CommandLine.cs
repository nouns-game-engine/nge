using System.Reflection;
using Microsoft.Extensions.Configuration;
using Nouns.CLI.Configuration;

namespace Nouns.CLI
{
    public static class CommandLine
    {
        private static bool EndOfSubArguments(this Queue<string> arguments) => arguments.Count == 0 ||
                                                                               arguments.Peek().StartsWith("--");

        public static void ProcessArguments(IConfiguration config, params string[] args)
        {
            var assetRootDir = config.GetSection("locations")["assetDirectory"];
            Console.Out.WriteLine("Asset root directory: " + assetRootDir);
            Console.Out.WriteLine();

            var arguments = new Queue<string>(args);

            while (arguments.Count > 0)
            {
                var arg = arguments.Dequeue();

                switch (arg.ToLower())
                {
                    case "games":

                        if (!EndOfSubArguments(arguments))
                        {
                            var command = arguments.Dequeue();
                            switch (command.ToLowerInvariant())
                            {
                                case "add":
                                    AddGame(arguments);
                                    break;

                                case "list":
                                    ListGames(config);
                                    break;
                            }

                            return;
                        }

                        ListGames(config);
                        break;
                    case "stat":
                        if (EndOfSubArguments(arguments))
                        {
                            Console.Error.WriteLine("no stat object specified");
                        }
                        else
                        {
                            var target = arguments.Dequeue();
                            switch (target)
                            {
                                case "version":
                                    Console.Out.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                                    break;
                                default:
                                    Console.Error.WriteLine("unrecognized stat '" + target + "'");
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.Error.WriteLine("unrecognized command line parameter '" + arg + "' (position " + (args.Length - arguments.Count - 1) + ")");
                        break;
                }
            }
        }

        private static void ListGames(IConfiguration config)
        {
            var games = config.GetSection("games");
            var gameCount = 0;
            foreach (var game in games.GetChildren())
            {
                gameCount++;

                Console.WriteLine(game.Value);
            }

            if (gameCount == 0)
                Console.Out.WriteLine("no games found.");
            else
                Console.Out.WriteLine("(" + gameCount + " results)");
        }

        private static void AddGame(Queue<string> arguments)
        {
            if (EndOfSubArguments(arguments))
            {
                Console.Error.WriteLine("no game name specified");
            }
            else
            {
                var gameName = arguments.Dequeue().ToLowerInvariant();
                if (Config.TryAddGame(gameName))
                    Console.Out.WriteLine("added game " + gameName);
                else
                    Console.Error.WriteLine("could not add game " + gameName);
            }
        }
    }
}