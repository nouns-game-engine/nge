using NGE.Engine.InputManagement;

namespace NGE.Engine;

public sealed class GameStateManager
{
    private readonly Definitions definitions;
    public readonly UpdateContext updateContext;

    private GameState gameState;
    public GameState GameState => gameState;

    public GameStateManager(Definitions definitions, GameState gameState, UpdateContext updateContext)
    {
        this.definitions = definitions;
        this.gameState = gameState;
        this.updateContext = updateContext;
    }

    public void Update(MultiInputState input)
    {
        gameState.FillUpdateContext(updateContext, input);
        gameState.Update(updateContext, input);
    }

    #region Events

    public event Action? OnGameStateChanged;

    public void ReplaceGameState(GameState gameState)
    {
        this.gameState = gameState;
        GameStateChanged();
    }

    private void GameStateChanged()
    {
        OnGameStateChanged?.Invoke();
    }

    #endregion
}