using Microsoft.Xna.Framework.Graphics;
using System;

namespace Nouns.CLI
{
    internal sealed class HeadlessGraphicsDeviceService : IGraphicsDeviceService
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
    }
}