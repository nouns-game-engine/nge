using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualTests;

public class NounPart
{
    public Rectangle Rectangle { get; set; }
    public Texture2D Texture { get; set; }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Texture, new Vector2(position.X + Rectangle.X, position.Y + Rectangle.Y), Color.White);
    }
}