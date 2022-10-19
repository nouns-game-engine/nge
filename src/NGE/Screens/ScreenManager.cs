using Microsoft.Xna.Framework.Graphics;

namespace NGE.Screens
{
    internal sealed class ScreenManager
    {
        private readonly NounsGame game;
        private readonly ScreenContext screenContext;

        public ScreenManager(NounsGame game, ScreenContext screenContext)
        {
            this.game = game;
            this.screenContext = screenContext;
        }

        public void Update()
        {
            
        }

        public RenderTarget2D? Render()
        {
            return null;
        }
    }
}
