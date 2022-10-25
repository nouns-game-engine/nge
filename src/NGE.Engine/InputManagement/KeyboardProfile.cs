using Microsoft.Xna.Framework.Input;

namespace NGE.Engine.InputManagement;

public class KeyboardProfile<TPlayerButton> where TPlayerButton : Enum
{
    public string? Name { get; set; }
    public InputProfileType Type { get; set; }
    public KeyMapping<TPlayerButton>[][] KeyboardMap { get; set; } = null!;

    private Keys[][]? inputKeyboardMap;

    public Keys[][]? InputKeyboardMap
    {
        get
        {
            if (inputKeyboardMap == null)
                Sync();
            return inputKeyboardMap;
        }
    }

    public void Sync()
    {
        var profiles = new Keys[KeyboardMap.Length][];
        for (var i = 0; i < KeyboardMap.Length; i++)
        {
            var profile = new Keys[KeyboardMap[i].Length];
            for (var j = 0; j < KeyboardMap[i].Length; j++)
            {
                var layout = KeyboardMap[i].OrderBy(x => x.Key).ToArray();
                profile[j] = layout[j].Value;
            }
            profiles[i] = profile;
        }
        inputKeyboardMap = profiles;
    }
}