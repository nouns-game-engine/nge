namespace Nouns.Engine.Pixels;

public sealed class UpdateContext
{
    public Definitions Definitions { get { return GameState.definitions; } }

    #region Non-Retained Data

    public GameState? GameState { get; set; }

    public void Reset()
    {
        GameState = null;
    }

    #endregion


}