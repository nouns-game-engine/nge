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
}