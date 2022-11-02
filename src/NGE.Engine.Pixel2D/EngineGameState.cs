using NGE.Engine.InputManagement;

namespace NGE.Engine.Pixel2D
{
    public class EngineGameState : GameState
    {
        public List<Actor> actors = new();

        public EngineGameState(Definitions definitions) : base(definitions)
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

        public override void Update(UpdateContext updateContext, MultiInputState currentInput)
        {
            base.Update(updateContext, currentInput);

            lastInput = currentInput;
        }
    }
}
