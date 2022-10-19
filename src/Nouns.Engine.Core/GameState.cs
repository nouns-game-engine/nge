namespace Nouns.Engine.Core;

public abstract class GameState
{
    public readonly Definitions definitions;

    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }
}

public abstract class GameState<TUpdateContext, TDrawContext> : GameState
    where TUpdateContext : UpdateContext 
    where TDrawContext : DrawContext
{
    public virtual void Update(TUpdateContext updateContext)
    {
        updateContext.Reset();
        updateContext.GameState = this;
    }

    public virtual void Draw(TDrawContext drawContext) { }

    protected GameState(Definitions definitions) : base(definitions)

    {
    }
}