using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VisualTests;

public class Noun
{
    public Vector2 Position { get; set; }

    public NounPart Head { get; set; }
    public NounPart Body { get; set; }
    public NounPart Glasses { get; set; }
    public NounPart Accessory { get; set; }
    public NounPart Legs { get; set; }

    public void Draw(SpriteBatch spriteBatch, int scale)
    {
        Head.Draw(spriteBatch, Position, scale);
        Body.Draw(spriteBatch, Position, scale);
        Glasses.Draw(spriteBatch, Position, scale);
        Accessory.Draw(spriteBatch, Position, scale);
        Legs.Draw(spriteBatch, Position, scale);
    }
}