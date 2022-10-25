namespace NGE.Engine.Pixel2D;

public sealed class PlayerController : IController
{
    public readonly byte playerIndex;
    public Rumble rumble;

    public PlayerController(byte playerIndex)
    {
        this.playerIndex = playerIndex;
        rumble = new Rumble { range = 20 };
    }
}