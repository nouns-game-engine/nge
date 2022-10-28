namespace NGE.Engine.Pixel3D;

public class EngineGameState : GameState<EngineUpdateContext, EngineDrawContext>
{
    public readonly List<Actor> actors = new();

    public EngineGameState(Definitions definitions) : base(definitions)
    {

    }

    public override void Update(EngineUpdateContext updateContext)
    {
        base.Update(updateContext);

        foreach (var actor in actors)
            actor.Update(updateContext);
    }

    public override void Draw(EngineDrawContext drawContext)
    {
        base.Draw(drawContext);

        drawContext.BeginNoTransform();
        foreach (var actor in actors)
            actor.Draw(drawContext);

        drawContext.End();
    }
}