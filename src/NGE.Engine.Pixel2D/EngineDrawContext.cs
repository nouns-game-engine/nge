using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixel2D;

public sealed class EngineDrawContext : DrawContext
{
    public EngineDrawContext(SpriteBatch sb, SpriteFont defaultFont, Texture2D uiTexture, Texture2D whitePixel) 
        : base(sb, defaultFont, uiTexture, whitePixel) { }
}