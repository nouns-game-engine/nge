using System.Diagnostics;

namespace NGE.Engine.InputManagement;

public struct MultiInputState
{
    public InputState player1;
    public InputState player2;
    public InputState player3;
    public InputState player4;

    public const int Count = 4;

    public InputState this[int playerNumber]
    {
        get
        {
            switch (playerNumber)
            {
                case 0: return player1;
                case 1: return player2;
                case 2: return player3;
                case 3: return player4;
                default:
                    Debug.Assert(false);
                    return default;
            }
        }
        set
        {
            switch (playerNumber)
            {
                case 0: player1 = value; break;
                case 1: player2 = value; break;
                case 2: player3 = value; break;
                case 3: player4 = value; break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}