using NGE.Engine.InputManagement;

namespace NGE.Engine;

public abstract class UpdateContext
{
    public Definitions? Definitions => GameState?.definitions;

    public readonly LocalSettings localSettings;

    protected UpdateContext(LocalSettings localSettings)
    {
        this.localSettings = localSettings;
        AllPlayerInputs = new PlayerInput[MultiInputState.Count];
    }

    #region Non-Retained Data

    public GameState? GameState { get; set; }
    public IRandomProvider random = null!;
    public PlayerInput[] AllPlayerInputs { get; set; } = null!;

    public virtual void Reset()
    {
        GameState = null!;
        random = null!;
        Array.Clear(AllPlayerInputs, 0, AllPlayerInputs.Length);
    }

    #endregion
}