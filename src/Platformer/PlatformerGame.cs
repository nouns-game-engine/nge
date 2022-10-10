﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Assets.Core;
using Nouns.Core;
using Nouns.Core.Configuration;
using Nouns.Editor;
using Nouns.Engine.Pixels;

namespace Platformer
{
    public class PlatformerGame : IGame
    {
        private readonly IEditingContext editContext;

        private GameState gameState = null!;
        private UpdateContext updateContext = null!;
        private DrawContext drawContext = null!;
        private EditorAssetManager assetManager = null!;
        private Definitions definitions = null!;


        public PlatformerGame(IEditingContext editContext)
        {
            this.editContext = editContext;
        }

        public void Initialize(GameServiceContainer services)
        {
            definitions = new Definitions();
            gameState = new PlatformerGameState(definitions);
            assetManager = services.GetRequiredService<EditorAssetManager>();
            Content = services.GetRequiredService<ContentManager>();
        }

        public ContentManager Content { get; set; } = null!;

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
            Engine.Initialize(typeof(Cloud).Assembly);

            // simulate background loading
            // var backgroundTasks = new List<Task>();
            // backgroundTasks.Add(Task.Delay(TimeSpan.FromSeconds(10)));
            // return backgroundTasks.ToArray();
                        
            var sprite = new Sprite(Content.Load<Texture2D>("cloud-large"));
            var cel = new Cel(sprite);
            var frame = new AnimationFrame();
            frame.layers.Add(cel);
            var animation = new Animation();
            animation.Frames.Add(frame);
            var animationSet = new AnimationSet();
            animationSet.animations.Add(animation);
            
            var thing = new Thing(animationSet, new Position(100, 100), false);
            
            var level = new Level();
            level.things.Add(thing);
            definitions.levels.Add(level);

            var cloud = new Cloud(thing, updateContext);
            gameState.actors.Add(cloud);

            editContext.EditObject(cloud);

            assetManager.UserStartTracking(animationSet);

            return Array.Empty<Task>();
        }

        public void OnFinishedBackgroundLoading(SpriteBatch sb)
        {
            drawContext = new DrawContext(sb);
            updateContext = new UpdateContext();
        }

        public void Reset()
        {
            gameState = new PlatformerGameState(definitions);
        }
    }
}