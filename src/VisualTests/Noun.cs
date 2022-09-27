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

    public void Draw(SpriteBatch spriteBatch)
    {
        Head.Draw(spriteBatch, Position);
        Body.Draw(spriteBatch, Position);
        Glasses.Draw(spriteBatch, Position);
        Accessory.Draw(spriteBatch, Position);
        Legs.Draw(spriteBatch, Position);
    }
}