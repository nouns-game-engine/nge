using Microsoft.Xna.Framework;

namespace NGE.Engine.Pixels;

public class AnimationFrame
{
    public int delay;
    public List<Cel> layers = new();

    public void Draw(DrawContext drawContext, Position position, bool flipX)
    {
        Draw(drawContext, position, flipX, Color.White);
    }

    public void Draw(DrawContext drawContext, Position position, bool flipX, Color color)
    {
        foreach (var layer in layers)
            layer.Draw(drawContext, position, flipX, color);
    }
}