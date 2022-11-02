using NGE.Engine.InputManagement;

namespace NGE.Engine;

public abstract class GameState
{
    public int frameCounter;

    public readonly Definitions definitions;
    
    protected GameState(Definitions definitions)
    {
        this.definitions = definitions;
    }

    public MultiInputState lastInput;

    public virtual void Update(UpdateContext updateContext, MultiInputState currentInput)
    {
        frameCounter++;
    }

    public readonly IRandomProvider random = new SysNetRandom();

    public virtual void FillUpdateContext(UpdateContext updateContext, MultiInputState currentInput)
    {
        updateContext.Reset();

        updateContext.GameState = this;
        updateContext.random = random;

        for (var i = 0; i < MultiInputState.Count; i++)
            updateContext.AllPlayerInputs[i] = new PlayerInput(lastInput[i], currentInput[i]);
    }
}