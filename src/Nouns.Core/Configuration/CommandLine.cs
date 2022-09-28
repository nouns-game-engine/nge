using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Nouns.Core.Configuration
{
    public static class CommandLine
    {
        private static bool EndOfSubArguments(this Queue<string> arguments) => arguments.Count == 0 ||
                                                                               arguments.Peek().StartsWith("--");

        public static void ProcessArguments(ref IConfiguration configuration, params string[] args)
        {
            var assetRootDir = configuration.GetSection("locations")["assetDirectory"];
            Console.Out.WriteLine($"Asset root directory: {assetRootDir}");
            Console.Out.WriteLine();

            var arguments = new Queue<string>(args);

            while (arguments.Count > 0)
            {
                var arg = arguments.Dequeue();

                switch (arg.ToLower())
                {
                    case "--config":
                        if (EndOfSubArguments(arguments))
                        {
                            Console.Error.WriteLine("no config options specified");
                        }
                        else
                        {
                            var target = arguments.Dequeue();
                            switch (target)
                            {
                                case "reset":
                                    if (File.Exists(Constants.DefaultConfigFileName))
                                        File.Delete(Constants.DefaultConfigFileName);
                                    configuration = Config.GetOrCreateConfiguration();
                                    break;
                                case "set":
                                    if (EndOfSubArguments(arguments))
                                        Console.Error.WriteLine("missing set key and value");
                                    var key = arguments.Dequeue();
                                    if (EndOfSubArguments(arguments))
                                        Console.Error.WriteLine("missing set value");
                                    var value = arguments.Dequeue();
                                    configuration[key] = value;
                                    break;
                                default:
                                    Console.Error.WriteLine($"unrecognized command '{target}'");
                                    break;
                            }
                        }
                        break;

                    case "--imgui":
                        if (EndOfSubArguments(arguments))
                        {
                            Console.Error.WriteLine("no IMGUI options specified");
                        }
                        else
                        {
                            var target = arguments.Dequeue();
                            switch (target)
                            {
                                case "reset":
                                    if(File.Exists("imgui.ini"))
                                        File.Delete("imgui.ini");
                                    break;
                                default:
                                    Console.Error.WriteLine($"unrecognized command '{target}'");
                                    break;
                            }
                        }
                        break;

                    case "--stat":
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