using Microsoft.Extensions.Configuration;
using Tomlyn.Syntax;

namespace Nouns.Core.Configuration
{
    public static class Config
    {
        private static IConfigurationRoot configuration = null!;
        private static string configFilePath = null!;

        public static IConfiguration GetOrCreateConfiguration(string configFileName)
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
            var document = new DocumentSyntax
            {
                Tables =
                {
                    new TableSyntax("web3")
                    {
                        Items =
                        {
                            {"rpcUrl", "http://localhost:8545"},
                        }
                    },
                    new TableSyntax("locations")
                    {
                        Items =
                        {
                            {"assetDirectory", Directory.GetCurrentDirectory()},
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
    }
}