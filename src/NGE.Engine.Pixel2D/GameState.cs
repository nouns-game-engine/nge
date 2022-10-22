namespace NGE.Engine.Pixel2D
{
    public class GameState
    {
        public int frameCounter;
        public List<Actor> actors = new();

        public void Update()
        {
            frameCounter++;
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
