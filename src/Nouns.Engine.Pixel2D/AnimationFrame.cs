using Microsoft.Xna.Framework;

namespace Nouns.Engine.Pixel2D;

public class AnimationFrame
{
    public int delay;
    public List<Cel> layers = new();

    public void Draw(PixelsDrawContext drawContext, Position position, bool flipX)
    {
        Draw(drawContext, position, flipX, Color.White);
    }

    public void Draw(PixelsDrawContext drawContext, Position position, bool flipX, Color color)
    {
        foreach (var layer in layers)
            layer.Draw(drawContext, position, flipX, color);
    }
}