namespace Nouns.Engine.Pixels;

public sealed class UpdateContext
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