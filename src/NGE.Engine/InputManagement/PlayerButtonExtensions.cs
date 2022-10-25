namespace NGE.Engine.InputManagement;

public static class PlayerButtonExtensions
{
    public static InputState AsInput<TPlayerButton>(this TPlayerButton button) where TPlayerButton : Enum
    {
        return (InputState) (1u << (int)(object) button);
    }
}