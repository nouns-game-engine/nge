namespace Nouns.Engine.Core;

public abstract class UpdateContext
{
    public Definitions Definitions => GameState.definitions;

    #region Non-Retained Data

    public GameState GameState { get; set; } = null!;

    public void Reset()
    {
        GameState = null!;
    }

    #endregion
}