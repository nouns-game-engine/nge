using Microsoft.Xna.Framework;

namespace NGE.Engine.Pixels;

public class AnimationFrame
{
    public int delay;
    public List<Cel> layers = new();

    public void Draw(DrawContext drawContext, Position position, bool flipX, float radians = 0f)
    {
        Draw(drawContext, position, flipX, Color.White, radians);
    }

    public void Draw(DrawContext drawContext, Position position, bool flipX, Color color, float radians = 0f)
    {
        foreach (var layer in layers)
            layer.Draw(drawContext, position, flipX, color, radians);
    }
}