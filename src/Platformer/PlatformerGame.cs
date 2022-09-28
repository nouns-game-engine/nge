using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Engine.Pixels;
using Nouns.StateMachine;

namespace Platformer
{
    public class PlatformerGame
    {
        private GameState gameState;
        private UpdateContext updateContext;
        private DrawContext drawContext;

        public ContentManager Content { get; }

        public PlatformerGame(ContentManager content)
        {
            Content = content;
        }

        public void Initialize()
        {
            gameState = new GameState();
        }

        public void Update()
        {
            gameState.Update(updateContext);
        }

        public void Draw(RenderTarget2D renderTarget)
        {
            renderTarget.GraphicsDevice.Clear(Color.SkyBlue);

            gameState.Draw(drawContext);
        }

#if WASM
        public void BackgroundLoad()
        {

        }
#endif
        
        public Task[] StartBackgroundLoading()
        {
            // simulate background loading
            // var backgroundTasks = new List<Task>();
            // backgroundTasks.Add(Task.Delay(TimeSpan.FromSeconds(10)));
            // return backgroundTasks.ToArray();

            StateProvider.Setup(new[] { typeof(Cloud).Assembly });

            var sprite = new Sprite(Content.Load<Texture2D>("cloud-large"));
            var cel = new Cel(sprite);
            var frame = new AnimationFrame();
            frame.layers.Add(cel);
            var animation = new Animation();
            animation.Frames.Add(frame);
            var animationSet = new AnimationSet();
            animationSet.animations.Add(animation);
            var thing = new Thing(animationSet, new Position(100, 100), false);
            var cloud = new Cloud(thing, updateContext);
            gameState.actors.Add(cloud);

            return Array.Empty<Task>();
        }

        public void OnFinishedLoading(SpriteBatch sb)
        {
            drawContext = new DrawContext(sb);
            updateContext = new UpdateContext();
        }
    }
}