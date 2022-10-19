namespace Nouns.Engine.Core;

public abstract class GameState
{
    public readonly Definitions definitions;

    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }
}