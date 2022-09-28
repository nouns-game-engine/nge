using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;

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
                                    Console.Error.WriteLine($"unrecognized stat '{target}'");
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.Error.WriteLine($"unrecognized command line parameter '{arg}' (position {(args.Length - arguments.Count - 1)})");
                        break;
                }
            }
        }
    }
}