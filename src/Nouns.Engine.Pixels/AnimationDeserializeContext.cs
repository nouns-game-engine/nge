﻿using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Engine.Pixels
{
    public sealed class AnimationDeserializeContext
    {
        public AnimationDeserializeContext(BinaryReader br, GraphicsDevice graphicsDevice)
        {
            this.br = br;

            Version = br.ReadInt32();

            if (Version > AnimationSerializeContext.FormatVersion)
                throw new Exception("Tried to save asset with a version that is too new");

            GraphicsDevice = graphicsDevice;
        }

        public readonly BinaryReader br;

        public int Version { get; }

        public GraphicsDevice GraphicsDevice { get; }
    }
}
