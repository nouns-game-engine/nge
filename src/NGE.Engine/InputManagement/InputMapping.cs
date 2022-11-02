using NGE.Core;

namespace NGE.Engine.InputManagement;

public class InputMapping
{
    public static MultiInputState GetMultiInputState<TPlayerButton>(LocalSettings<TPlayerButton> localSettings) where TPlayerButton : Enum
    {
        var output = new MultiInputState();
        if (!Input.IsActive)
            return output;

        var keys = new MultiInputState();
        var keyboardMap = localSettings.inputBindings.KeyboardProfiles[localSettings.inputBindings.KeyboardProfileIndex].InputKeyboardMap;

        for (var i = 0; i < MultiInputState.Count; i++)
        {
            if(keyboardMap != null)
                keys[i] |= Input.keyboardState.MapInputs(keyboardMap[i]);
        }

        var gamepads = new MultiInputState();
        for (var i = 0; i < MultiInputState.Count; i++)
        {
            var gamePadMap = localSettings.inputBindings.GamePadProfiles[localSettings.inputBindings.GamePadProfileIndices[i]].InputGamePadMap;
            var gamePadMapAlt = localSettings.inputBindings.GamePadProfiles[localSettings.inputBindings.GamePadProfileIndices[i]].InputGamePadMapAlt;

            if(gamePadMap != null)
                gamepads[i] |= Input.GamePadState(i).MapInputs(gamePadMap);

            if(gamePadMapAlt != null)
                gamepads[i] |= Input.GamePadState(i).MapInputs(gamePadMapAlt);
        }

        for (var i = 0; i < MultiInputState.Count; i++)
            output[i] = keys[i] | gamepads[i];

        return output;
    }
}