namespace Nouns.Engine.Pixels;

public abstract class GameState
{
    public readonly Definitions definitions;
    public readonly List<Actor> actors = new();

    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }

    public void Update(UpdateContext updateContext)
    {
        updateContext.Reset();
        updateContext.GameState = this;

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