using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixel3D;

public sealed class EngineDrawContext : DrawContext
{
    public EngineDrawContext(SpriteBatch spriteBatch, SpriteFont defaultFont, Texture2D whitePixel) : base(spriteBatch, defaultFont, whitePixel) { }
}