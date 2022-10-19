using Microsoft.Xna.Framework;

namespace Nouns.Engine.Pixel2D;

public class Cel
{
    private readonly Sprite sprite;

    public Cel(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void Draw(PixelsDrawContext drawContext, Position position, bool flipX, Color color)
    {
        drawContext.DrawWorldNoTransform(sprite, position, color, flipX);
    }
}