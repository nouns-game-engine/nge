using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace NGE.Core.Configuration
{
    public static class CommandLine
    {
        public const string PrefixVerbose = "--";
        public const string PrefixSteamStyle = "+";

        private static readonly Dictionary<string, Func<IConfiguration, Queue<string>, IConfiguration>> commands = new(StringComparer.OrdinalIgnoreCase);

        static CommandLine()
        {
            commands.Add("config", ConfigOptions);
            commands.Add("imgui", ImGuiOptions);
            commands.Add("stat", StatOptions);
        }

        public static void AddCommand(string commandName, Func<IConfiguration, Queue<string>, IConfiguration> commandFunc)
        {
            commands[commandName] = commandFunc;
        }
        
        public static void ProcessArguments(ref IConfiguration configuration, params string[] args)
        {
            var arguments = new Queue<string>(args);

            while (arguments.Count > 0)
            {
                var commandName = arguments.Dequeue()
                    .TrimStart(PrefixVerbose.ToCharArray())
                    .TrimStart(PrefixSteamStyle.ToCharArray())
                    ;

                if (commands.TryGetValue(commandName, out var commandFunc))
                {
                    configuration = commandFunc(configuration, arguments);
                }
            }
        }

        private static IConfiguration StatOptions(IConfiguration configuration, Queue<string> arguments)
        {
            if (arguments.EndOfSubArguments())
            {
                Console.Error.WriteLine("no stat object specified");
            }
            else
            {
                var target = arguments.Dequeue();
                switch (target.ToLowerInvariant())
                {
                    case "version":
                        Console.Out.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
                        break;
                    default:
                        Console.Error.WriteLine($"unrecognized stat '{target}'");
                        break;
                }
            }

            return configuration;
        }

        private static IConfiguration ImGuiOptions(IConfiguration configuration, Queue<string> arguments)
        {
            if (arguments.EndOfSubArguments())
            {
                Console.Error.WriteLine("no ImGui options specified");
            }
            else
            {
                var target = arguments.Dequeue();
                switch (target.ToLowerInvariant())
                {
                    case "reset":
                        if (File.Exists("imgui.ini"))
                            File.Delete("imgui.ini");
                        break;
                    default:
                        Console.Error.WriteLine($"unrecognized command '{target}'");
                        break;
                }
            }

            return configuration;
        }

        private static IConfiguration ConfigOptions(IConfiguration configuration, Queue<string> arguments)
        {
            if (arguments.EndOfSubArguments())
            {
                Console.Error.WriteLine("no config options specified");
            }
            else
            {
                var target = arguments.Dequeue();
                switch (target.ToLowerInvariant())
                {
                    case "reset":
                        if (File.Exists(Constants.DefaultConfigFileName))
                            File.Delete(Constants.DefaultConfigFileName);
                        return Config.GetOrCreateConfiguration();
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

            return configuration;
        }

        public static bool EndOfSubArguments(this Queue<string> arguments) => arguments.Count == 0 ||
                                                                              arguments.Peek().StartsWith(PrefixVerbose) ||
                                                                              arguments.Peek().StartsWith(PrefixSteamStyle);
    }
}