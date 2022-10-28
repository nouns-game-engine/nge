using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixels;

public static class SpriteExtensions
{
    public static void DrawWorldNoTransform(this SpriteBatch sb, Sprite sprite, Position position, Color color, bool flipX)
    {
        DrawWorldNoTransform(sb, sprite.texture, position, sprite.sourceRectangle, color, flipX, Vector2.One);
    }

    public static void DrawWorldNoTransform(this SpriteBatch sb, Texture2D texture, Position position, Rectangle sourceRectangle, Color color, bool flipX, Vector2 scale)
    {
        var effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        sb.Draw(texture, position.ToDisplayNoTransform, sourceRectangle, color, 0, Vector2.One, scale, effects, 0);
    }

    public static void DrawWorld(this SpriteBatch sb, Texture2D texture, Position position, Rectangle sourceRectangle, Color color, Vector2 origin, bool flipX, Vector2 scale)
    {
        var effects = SpriteEffects.None;
        if (flipX)
        {
            effects = SpriteEffects.FlipHorizontally;
            origin.X = sourceRectangle.Width - origin.X - 1;
        }
        sb.Draw(texture, position.ToDisplayNoTransform, sourceRectangle, color, 0, origin, scale, effects, 0);
    }

    public static void DrawWorld(this SpriteBatch sb, Sprite sprite, Position position, Color color, bool flipX)
    {
        DrawWorld(sb, sprite.texture, position, sprite.sourceRectangle, color, sprite.DrawOrigin, flipX, Vector2.One);
    }
}