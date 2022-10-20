using Microsoft.Xna.Framework;

namespace NGE.Engine.Pixel2D;

public class Cel
{
    private readonly Sprite sprite;

    // ReSharper disable once UnusedMember.Global (Serialization)
    public Cel() { }

    public Cel(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void Draw(PixelsDrawContext drawContext, Position position, bool flipX, Color color)
    {
        drawContext.DrawWorldNoTransform(sprite, position, color, flipX);
    }
}