using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace NGE.Engine.InputManagement;

public class GamePadProfile<TPlayerButton> where TPlayerButton : Enum
{
    public string? Name { get; set; }
    public InputProfileType Type { get; set; }

    public ButtonMapping<TPlayerButton>[] GamePadMap { get; set; } = null!;
    public ButtonMapping<TPlayerButton>[] GamePadMapAlt { get; set; } = null!;
    
    private Buttons[]? inputGamePadMap;

    public Buttons[]? InputGamePadMap
    {
        get
        {
            if (inputGamePadMap == null)
                Sync();
            return inputGamePadMap;
        }
    }


    private Buttons[]? inputGamePadMapAlt;
    public Buttons[]? InputGamePadMapAlt
    {
        get
        {
            if (inputGamePadMapAlt == null)
                SyncAlt();
            return inputGamePadMapAlt;
        }
    }

    public void Sync()
    {
        var profile = new Buttons[GamePadMap.Length];
        for (var i = 0; i < GamePadMap.Length; i++)
        {
            var layout = GamePadMap.OrderBy(x => x.Key).ToArray();
            profile[i] = layout[i].Value;
        }
        inputGamePadMap = profile;
    }

    public void SyncAlt()
    {
        var profile = new Buttons[GamePadMapAlt.Length];
        for (var i = 0; i < GamePadMapAlt.Length; i++)
        {
           var layout = GamePadMapAlt.OrderBy(x => x.Key).ToArray();
            profile[i] = layout[i].Value;
        }
        inputGamePadMapAlt = profile;
    }
}