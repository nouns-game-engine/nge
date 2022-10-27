using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Graphics;
using NGE.Core;
using NGE.Core.Configuration;
using NGE.Graphics;

namespace NGE.CLI
{
    internal static class Program
    {
        // ReSharper disable once NotAccessedField.Local
        private static GraphicsDevice graphicsDevice = null!;

        // ReSharper disable once NotAccessedField.Local
        private static IServiceContainer services = null!;

        private static void Main(string[] args)
        {
            Masthead.Print();
            Bootstrap.Init();

            graphicsDevice = Headless.AcquireGraphicsDevice();
            services = Headless.AcquireServices();

            var configuration = Config.GetOrCreateConfiguration();
            CommandLine.ProcessArguments(ref configuration, args);

            var assetRootDir = configuration.GetSection("locations")["assetDirectory"];
            Console.Out.WriteLine($"Asset root directory: {assetRootDir}");
            Console.Out.WriteLine();
        }
    }
}