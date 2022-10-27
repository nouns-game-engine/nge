using Microsoft.Extensions.Configuration;
using Tomlyn.Syntax;

namespace NGE.Core.Configuration
{
    public static class Config
    {
        private static IConfigurationRoot configuration = null!;
        private static string configFilePath = null!;

        public static IConfiguration GetOrCreateConfiguration(string configFileName = Constants.DefaultConfigFileName)
        {
            var workingDir = Directory.GetCurrentDirectory();
            configFilePath = Path.Combine(workingDir, configFileName);

            if (!File.Exists(configFilePath))
                CreateDefaultConfigFile();

            var builder = new ConfigurationBuilder()
                .SetBasePath(workingDir)
                .AddTomlFile(configFilePath, optional: false, reloadOnChange: true);

            configuration = builder.Build();
            return configuration;
        }

        private static void CreateDefaultConfigFile()
        {
            var excludedAssemblies = new HashSet<string>
            {
                "Microsoft.*",
                "System.*",
                "NBitcoin.*",
                "Nethereum.*",
                "Magick.NET*",
                "Svg",
                "Tomlyn",
                "runtime.osx.*",
                "FNA",
                "SixLabors.ImageSharp",
                "ImGui.NET",
                "BouncyCastle.Crypto",
                "ExCSS",
                "Fizzler",
                "Newtonsoft.Json",
                "SharpGLTF.*"
            };

            var excludedAssembliesString = string.Join(';', excludedAssemblies);
            
            var document = new DocumentSyntax
            {
                Tables =
                {
                    new TableSyntax("web3")
                    {
                        Items =
                        {
                            {"rpcUrl", "http://localhost:8545"}
                        }
                    },
                    new TableArraySyntax("web3.knownContracts")
                    {
                        // ReSharper disable StringLiteralTypo
                        // ReSharper disable once CommentTypo

                        Items =
                        {
                            {"Nouns", @"0x9c8ff314c9bc7f6e59a9d9225fb22946427edc03"},
                            {"CrypToadz", @"0x1cb1a5e65610aeff2551a50f76a87a7d3fb649c6"}
                        }
                    },
                    new TableSyntax("editor")
                    {
                        Items =
                        {
                            {"excludeAssemblies", excludedAssembliesString}
                        }
                    },
                    new TableSyntax("locations")
                    {
                        Items =
                        {
                            {"assetDirectory", Path.Combine(Directory.GetCurrentDirectory(), "Content")}
                        }
                    },
                    new TableSyntax("options")
                    {
                        Items =
                        {
                            {"liveReload", true}
                        }
                    },
                    new TableSyntax("games")
                    {
                        Items =
                        {
                            {"platformer", "C:\\src\\nounsgame\\src\\Platformer\\bin\\Debug\\net6.0\\platformer.dll"}
                        }
                    }
                }
            };

            Save(document);
        }

        private static void Save(SyntaxNode document)
        {
            using var fs = File.OpenWrite(configFilePath);
            using var sw = new StreamWriter(fs);
            document.WriteTo(sw);
        }

        public static void Save()
        {
            foreach(var provider in configuration.Providers)
                if(provider is TomlConfigurationProvider toml)
                    toml.Save();
        }

        public static void SaveAs(string path)
        {
            foreach (var provider in configuration.Providers)
                if (provider is TomlConfigurationProvider toml)
                    toml.SaveAs(path);
        }
    }
}