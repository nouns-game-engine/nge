using Microsoft.Xna.Framework.Graphics;
using NGE.Core;
using NGE.Core.Serialization;

namespace NGE.Engine.Pixels.Serialization
{
    public sealed class AnimationDeserializeContext : IDeserializeContext
    {
        public int Version { get; }
        public GraphicsDevice GraphicsDevice { get; }

        public readonly BinaryReader br;

        public AnimationDeserializeContext(BinaryReader br, IServiceProvider serviceProvider)
        {
            this.br = br;

            Version = br.ReadInt32();

            if (Version > AnimationSerializeContext.FormatVersion)
                throw new Exception("Tried to save asset with a version that is too new");

            GraphicsDevice = serviceProvider.GetGraphicsDevice();
        }
    }
}
