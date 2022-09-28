using System;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core.Configuration;

namespace Nouns.CLI
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

            Bootstrap();

            graphicsDevice = AcquireGraphicsDevice(out var serviceContainer);
            services = serviceContainer;

            CommandLine.ProcessArguments(Config.GetOrCreateConfiguration("config.toml"), args);
        }

        #region Bootstrap

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetDllDirectory(string lpPathName);

        public static void Bootstrap()
        {
            // https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#64-bit-support
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetDllDirectory(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    Environment.Is64BitProcess ? "x64" : "x86"
                ));
            }

            // https://github.com/FNA-XNA/FNA/wiki/7:-FNA-Environment-Variables#fna_graphics_enable_highdpi
            // NOTE: from documentation: 
            //       Lastly, when packaging for macOS, be sure this is in your app bundle's Info.plist:
            //           <key>NSHighResolutionCapable</key>
            //           <string>True</string>
            Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
        }

        public sealed class NoGame : Game
        {
            // ReSharper disable once NotAccessedField.Local
            private readonly GraphicsDeviceManager graphics;
            public NoGame() => graphics = new GraphicsDeviceManager(this);
            protected override void Draw(GameTime gameTime) { }
        }

        #endregion

        #region GraphicsDevice

        private static GraphicsDevice AcquireGraphicsDevice(out ServiceContainer serviceContainer)
        {
            var game = new NoGame();
            game.RunOneFrame();
            serviceContainer = new ServiceContainer();
            serviceContainer.AddService(typeof(IGraphicsDeviceService), new HeadlessGraphicsDeviceService(game.GraphicsDevice));
            return game.GraphicsDevice;
        }

        #endregion
    }
}