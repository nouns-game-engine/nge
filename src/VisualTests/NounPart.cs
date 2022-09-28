using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualTests;

public class NounPart
{
    public string Name { get; set; }
    public Rectangle Rectangle { get; set; }
    public Texture2D Texture { get; set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, int scale)
    {
        var x = position.X + Rectangle.X * scale;
        var y = position.Y + Rectangle.Y * scale;

        position = new Vector2(x, y);

        spriteBatch.Draw(Texture, position, null, Color.White, 0,
            Vector2.Zero, scale, SpriteEffects.None, 0);
    }
}