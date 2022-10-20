using Microsoft.Xna.Framework.Graphics;
using NGE.Core.Serialization;

namespace Nouns.Engine.Pixel2D.Serialization
{
    public sealed class AnimationDeserializeContext : IDeserializeContext
    {
        public int Version { get; }
        public GraphicsDevice GraphicsDevice { get; }

        public readonly BinaryReader br;

        public AnimationDeserializeContext(BinaryReader br, GraphicsDevice graphicsDevice)
        {
            this.br = br;

            Version = br.ReadInt32();

            if (Version > AnimationSerializeContext.FormatVersion)
                throw new Exception("Tried to save asset with a version that is too new");

            GraphicsDevice = graphicsDevice;
        }
    }
}
