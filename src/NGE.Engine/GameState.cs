namespace NGE.Engine;

public abstract class GameState
{
    public int frameCounter;

    public readonly Definitions definitions;
    
    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }

    public virtual void Update(UpdateContext updateContext)
    {
        frameCounter++;
    }

    public readonly IRandomProvider random = new SysNetRandom();

    public virtual void FillUpdateContext(UpdateContext updateContext)
    {
        updateContext.Reset();

        updateContext.GameState = this;
        updateContext.random = random;
    }
}