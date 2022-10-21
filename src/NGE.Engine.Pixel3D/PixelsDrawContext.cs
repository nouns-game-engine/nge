using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixel3D;

public sealed class PixelsDrawContext : DrawContext
{
    public PixelsDrawContext(SpriteBatch spriteBatch) : base(spriteBatch) { }

    public void DrawWorldNoTransform(Sprite sprite, Position position, Color color, bool flipX)
    {
        sb.DrawWorldNoTransform(sprite, position, color, flipX);
    }
}