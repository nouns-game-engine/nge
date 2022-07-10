using Microsoft.Extensions.Configuration;
using System.IO;
using Tomlyn;
using Tomlyn.Syntax;

namespace Nouns.CLI.Configuration
{
    public static class Config
    {
        private static IConfigurationRoot _configuration = null!;
        private static string _configFilePath = null!;

        public static IConfiguration GetOrCreateConfiguration(string configFileName)
        {
            var workingDir = Directory.GetCurrentDirectory();
            _configFilePath = Path.Combine(workingDir, configFileName);

            if (!File.Exists(_configFilePath))
                CreateDefaultConfigFile(_configFilePath);

            var builder = new ConfigurationBuilder()
                .SetBasePath(workingDir)
                .AddTomlFile(_configFilePath, optional: false, reloadOnChange: false);

            _configuration = builder.Build();
            return _configuration;
        }

        private static void CreateDefaultConfigFile(string configFilePath)
        {
            var document = new DocumentSyntax
            {
                Tables =
                {
                    new TableSyntax("locations")
                    {
                        Items =
                        {
                            {"assetDirectory", Directory.GetCurrentDirectory()},
                        }
                    }
                }
            };

            using var fs = File.OpenWrite(configFilePath);
            using var sw = new StreamWriter(fs);
            document.WriteTo(sw);
        }

        public static bool TryAddGame(string gameName)
        {
            var workingDir = Directory.GetCurrentDirectory();
            var configFilePath = Path.Combine(workingDir, _configFilePath);
            var document = Toml.Parse(File.ReadAllBytes(configFilePath));

            var found = false;
            foreach (var table in document.Tables)
            {
                if (table.Name.ToString() == "games")
                {
                    found = true;
                }
            }

            if (!found)
            {
                var table = new TableArraySyntax("games")
                {
                    Items =
                    {
                        {"name", gameName}
                    }
                };

                document.Tables.Add(table);

                Save(configFilePath, document);
                _configuration.Reload();
                return true;
            }

            return false;
        }

        private static void Save(string configFilePath, SyntaxNode document)
        {
            using var fs = File.OpenWrite(configFilePath);
            using var sw = new StreamWriter(fs);
            document.WriteTo(sw);
        }
    }
}