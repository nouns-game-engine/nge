using Microsoft.Xna.Framework;

namespace NGE.Engine.Pixels;

public class Cel
{
    private readonly Sprite sprite;

    // ReSharper disable once UnusedMember.Global (Serialization)
    public Cel() { }

    public Cel(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void Draw(DrawContext drawContext, Position position, bool flipX, Color color)
    {
        drawContext.sb.DrawWorldNoTransform(sprite, position, color, flipX);
    }
}