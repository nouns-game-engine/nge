namespace NGE.Engine;

public abstract class UpdateContext
{
    public Definitions? Definitions => GameState?.definitions;

    public readonly LocalSettings localSettings;

    protected UpdateContext(LocalSettings localSettings)
    {
        this.localSettings = localSettings;
    }

    #region Non-Retained Data

    public GameState? GameState { get; set; }
    public IRandomProvider random = null!;

    public void Reset()
    {
        GameState = null!;
        random = null!;
    }

    #endregion
}