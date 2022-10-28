using Microsoft.Xna.Framework;
using NGE.Core;

namespace NGE.Engine.Pixel2D
{
    public sealed class RumbleEffect
    {
        public const byte RumbleCountdownTime = 20;

        public int rumbleCountdown;
        public int lastRumbleUpdateFrame;

        public void Update(EngineGameState gameState)
        {
            if (!Input.IsActive || lastRumbleUpdateFrame == gameState.frameCounter)
            {
                if (rumbleCountdown > 0)
                    rumbleCountdown--;
            }
            else
            {
                rumbleCountdown = RumbleCountdownTime;
            }

            lastRumbleUpdateFrame = gameState.frameCounter;

            for (var i = 0; i < EngineConstants.MaxPlayers; i++)
            {
                var index = (PlayerIndex)i;

                if (rumbleCountdown > 0)
                {
                    float playerLow = 0;
                    float playerHigh = 0;

                    var playerController = gameState.GetPlayerController(i);
                    if (playerController != null)
                    {
                        playerLow = (playerController.rumble.low / (float)playerController.rumble.range).Clamp();
                        playerHigh = (playerController.rumble.high / (float)playerController.rumble.range).Clamp();
                    }

                    var left = playerLow.Clamp();
                    var right = playerHigh.Clamp();
                    var scale = (rumbleCountdown / (float)RumbleCountdownTime).MapFrom(0f, 0.8f).Clamp();

                    Input.GamePadRumble((int)index, left * scale, right * scale);
                }
                else
                {
                    Input.GamePadRumble((int)index, 0, 0);
                }
            }
        }
    }
}
