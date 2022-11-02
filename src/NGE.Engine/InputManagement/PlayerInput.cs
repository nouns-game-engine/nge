namespace NGE.Engine.InputManagement;

public struct PlayerInput
{
    public static readonly PlayerInput None = new();

    public PlayerInput(InputState last, InputState current)
    {
        this.current = current;
        this.last = last;
    }

    public const InputState DirectionMask = (InputState)0xFu;

    public InputState last;
    public InputState current;

    public bool IsUp<TPlayerButton>(TPlayerButton button) => !IsDown(button);
    public bool IsDown<TPlayerButton>(TPlayerButton button) => (current & (InputState)(1u << (int)(object) button)) != 0;
    
    public bool Changed<TPlayerButton>(TPlayerButton button) => (current & (InputState)(1u << (int)(object) button)) != (last & (InputState)(1u << (int)(object) button));
    public bool PreviousFrameDown<TPlayerButton>(TPlayerButton button) => (last & (InputState)(1u << (int)(object) button)) != 0;

    public bool WentUp<TPlayerButton>(TPlayerButton button) => !IsDown(button) & PreviousFrameDown(button);
    public bool WentDown<TPlayerButton>(TPlayerButton button) => IsDown(button) & !PreviousFrameDown(button);

    public bool AnythingWentDown() => !current.Equals(InputState.None) && last.Equals(InputState.None);

    public bool AnythingIsDown() => !current.Equals(InputState.None);

    public bool IsDownDirection<TPlayerButton>(TPlayerButton button) where TPlayerButton : Enum
    {
        if ((int)(object)button >= 4)
            return IsDown(button);
        return (current & DirectionMask) == button.AsInput();
    }

    public bool WentDownDirection<TPlayerButton>(TPlayerButton button) where TPlayerButton : Enum
    {
        if ((int)(object)button >= 4)
            return WentDown(button);
        return ((current & DirectionMask) == button.AsInput()) & ((last & DirectionMask) == 0);
    }
}