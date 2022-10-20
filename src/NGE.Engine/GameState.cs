namespace NGE.Engine;

public abstract class GameState
{
    public readonly Definitions definitions;

    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }
}