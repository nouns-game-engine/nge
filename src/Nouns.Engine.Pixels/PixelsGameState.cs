using Nouns.Engine.Core;

namespace Nouns.Engine.Pixels;

public class PixelsGameState : GameState<PixelsUpdateContext, PixelsDrawContext>
{
    public readonly List<Actor> actors = new();

    public PixelsGameState(Definitions definitions) : base(definitions)
    {

    }

    public override void Update(PixelsUpdateContext updateContext)
    {
        base.Update(updateContext);

        foreach (var actor in actors)
            actor.Update(updateContext);
    }

    public override void Draw(PixelsDrawContext drawContext)
    {
        base.Draw(drawContext);

        drawContext.BeginNoTransform();
        foreach (var actor in actors)
            actor.Draw(drawContext);

        drawContext.End();
    }
}