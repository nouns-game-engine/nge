using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core.Configuration;
using Nouns.Graphics.Core;

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

            graphicsDevice = HeadlessGraphicsDeviceService.AcquireGraphicsDevice(out var serviceContainer);
            services = serviceContainer;

            var configuration = Config.GetOrCreateConfiguration();
            CommandLine.ProcessArguments(ref configuration, args);
        }
    }
}