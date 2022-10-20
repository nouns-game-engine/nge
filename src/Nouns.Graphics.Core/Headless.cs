using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Graphics.Core
{
    public sealed class Headless : IGraphicsDeviceService
    {
        public GraphicsDevice GraphicsDevice { get; }

        public event EventHandler<EventArgs> DeviceCreated = (_, _) => { };
        public event EventHandler<EventArgs> DeviceDisposing = (_, _) => { };
        public event EventHandler<EventArgs> DeviceReset = (_, _) => { };
        public event EventHandler<EventArgs> DeviceResetting = (_, _) => { };

        public Headless(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public static GraphicsDevice graphicsDevice;
        public static IServiceContainer services;

        public static IServiceContainer AcquireServices()
        {
            if (services == null)
                AcquireGraphicsDevice();

            return services;
        }

        public static GraphicsDevice AcquireGraphicsDevice()
        {
            Bootstrap.Init();

            if (graphicsDevice == null)
            {
                var game = new NoGame();
                game.RunOneFrame();
                services = new ServiceContainer();
                services.AddService(typeof(IGraphicsDeviceService), new Headless(game.GraphicsDevice));
                graphicsDevice = game.GraphicsDevice;
            }

            return graphicsDevice;
        }
    }
}