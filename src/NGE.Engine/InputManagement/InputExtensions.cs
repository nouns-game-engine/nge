using Microsoft.Xna.Framework.Input;

namespace NGE.Engine.InputManagement;

public static class InputExtensions
{
    public static InputState MapInputs(this KeyboardState keyboardState, Keys[] keyboardMap)
    {
        if (keyboardMap.Length > 32)
            throw new NotSupportedException($"InputState can store up to 32 mappings, but {keyboardMap.Length} keyboard mappings exist");

        InputState output = 0;
        for (var i = 0; i < keyboardMap.Length; i++)
        {
            if (keyboardState.IsKeyDown(keyboardMap[i]))
                output |= (InputState)(1u << i);
        }

        return output;
    }

    public static InputState MapInputs(this GamePadState gamePadState, Buttons[] gamePadMap)
    {
        if (gamePadMap.Length > 32)
            throw new NotSupportedException(
                $"InputState can store up to 32 mappings, but {gamePadMap.Length} gamepad mappings exist");

        InputState output = 0;
        for (var i = 0; i < gamePadMap.Length; i++)
        {
            if (gamePadState.IsButtonDown(gamePadMap[i]))
                output |= (InputState)(1u << i);
        }

        return output;
    }

}