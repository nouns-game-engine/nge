namespace NGE.Engine.Pixel2D
{
    public class PixelGameState : GameState
    {
        public List<Actor> actors = new();

        public PixelGameState(Definitions definitions) : base(definitions)
        {

        }

        public PlayerController? GetPlayerController(int controllerIndex)
        {
            foreach (var actor in actors)
            {
                if (actor is ControllableActor controllable)
                {
                    if (controllable.controller is PlayerController playerController)
                    {
                        if(playerController.playerIndex == controllerIndex)
                            return playerController;
                    }
                }
            }

            return null;
        }
    }
}
