using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Engine.Pixels;

public static class SpriteExtensions
{
    public static void DrawWorldNoTransform(this SpriteBatch sb, Sprite sprite, Position position, Color color, bool flipX, float radians = 0f)
    {
        DrawWorldNoTransform(sb, sprite.texture, position, sprite.sourceRectangle, color, flipX, Vector2.One, radians);
    }

    public static void DrawWorldNoTransform(this SpriteBatch sb, Texture2D texture, Position position, Rectangle sourceRectangle, Color color, bool flipX, Vector2 scale, float radians = 0f)
    {
        var effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        sb.Draw(texture, position.ToDisplayNoTransform, sourceRectangle, color, radians, Vector2.One, scale, effects, 0);
    }
}