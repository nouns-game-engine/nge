using Nouns.Graphics.Pixels;

namespace Platformer;

public sealed class GameState
{
    public readonly List<Actor> actors = new();

    public void Update(UpdateContext updateContext)
    {
        foreach (var actor in actors)
            actor.Update(updateContext);
    }

    public void Draw(DrawContext drawContext)
    {
        drawContext.BeginNoTransform();
        foreach (var actor in actors)
            actor.Draw(drawContext);
        drawContext.End();
    }
}