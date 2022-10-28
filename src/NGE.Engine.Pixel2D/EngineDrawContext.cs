using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixel2D;

public sealed class EngineDrawContext : DrawContext
{
    public EngineDrawContext(SpriteBatch sb, Texture2D whitePixel) : base(sb, whitePixel) { }
}