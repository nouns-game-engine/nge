using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Graphics.Core
{
    public sealed class HeadlessGraphicsDeviceService : IGraphicsDeviceService
    {
        public GraphicsDevice GraphicsDevice { get; }

        public event EventHandler<EventArgs> DeviceCreated = (_, _) => { };
        public event EventHandler<EventArgs> DeviceDisposing = (_, _) => { };
        public event EventHandler<EventArgs> DeviceReset = (_, _) => { };
        public event EventHandler<EventArgs> DeviceResetting = (_, _) => { };

        public HeadlessGraphicsDeviceService(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public static GraphicsDevice AcquireGraphicsDevice(out ServiceContainer serviceContainer)
        {
            var game = new NoGame();
            game.RunOneFrame();
            serviceContainer = new ServiceContainer();
            serviceContainer.AddService(typeof(IGraphicsDeviceService), new HeadlessGraphicsDeviceService(game.GraphicsDevice));
            return game.GraphicsDevice;
        }
    }
}