namespace NGE.Engine.InputManagement;

public class InputBindings<TPlayerButton> where TPlayerButton : Enum
{
    public List<KeyboardProfile<TPlayerButton>> KeyboardProfiles { get; set; }
    public int KeyboardProfileIndex { get; set; }
    public List<GamePadProfile<TPlayerButton>> GamePadProfiles { get; set; }
    public int[] GamePadProfileIndices { get; set; }

    public InputBindings()
    {
        KeyboardProfileIndex = -1;
        KeyboardProfiles = new List<KeyboardProfile<TPlayerButton>>();
        GamePadProfiles = new List<GamePadProfile<TPlayerButton>>();
        GamePadProfileIndices = new[] { -1, -1, -1, -1 };
    }
    
    public void SyncBindings()
    {
        foreach (var keyboardProfile in KeyboardProfiles)
        {
            keyboardProfile.Sync();
        }

        foreach (var gamePadProfile in GamePadProfiles)
        {
            gamePadProfile.Sync();
            gamePadProfile.SyncAlt();
        }

        {
            if (KeyboardProfileIndex < -1 || KeyboardProfileIndex >= KeyboardProfiles.Count)
                KeyboardProfileIndex = KeyboardProfiles.Count == 0 ? -1 : 0;

            if (GamePadProfileIndices[0] < -1 || GamePadProfileIndices[0] >= GamePadProfiles.Count)
                GamePadProfileIndices[0] = GamePadProfiles.Count == 0 ? -1 : 0;
            if (GamePadProfileIndices[1] < -1 || GamePadProfileIndices[1] >= GamePadProfiles.Count)
                GamePadProfileIndices[1] = GamePadProfiles.Count == 0 ? -1 : 0;
            if (GamePadProfileIndices[2] < -1 || GamePadProfileIndices[2] >= GamePadProfiles.Count)
                GamePadProfileIndices[2] = GamePadProfiles.Count == 0 ? -1 : 0;
            if (GamePadProfileIndices[3] < -1 || GamePadProfileIndices[3] >= GamePadProfiles.Count)
                GamePadProfileIndices[3] = GamePadProfiles.Count == 0 ? -1 : 0;
        }
    }
}