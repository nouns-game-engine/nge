using Microsoft.Xna.Framework;

namespace Nouns.Graphics.Pixels;

public class Cel
{
    private readonly Sprite sprite;

    public Cel(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void Draw(DrawContext drawContext, Position position, bool flipX, Color color)
    {
        drawContext.DrawWorldNoTransform(sprite, position, color, flipX);
    }
}