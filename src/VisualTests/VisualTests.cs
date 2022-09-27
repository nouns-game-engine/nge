using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core;

namespace VisualTests
{
    internal class VisualTests : Game
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable (retained)
        private readonly GraphicsDeviceManager graphics;

        public VisualTests()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            Window.AllowUserResizing = false;
            IsMouseVisible = true;
        }

        private Noun noun;

        protected override void Initialize()
        {
            noun = new Noun
            {
                Position = new Vector2(graphics.PreferredBackBufferWidth / 2f - 250, graphics.PreferredBackBufferHeight / 2f - 250),
                Body = Content.Load<Texture2D>("body-computerblue"),
                Head = Content.Load<Texture2D>("head-aardvark"),
                Glasses = Content.Load<Texture2D>("glasses-hip-rose"),
                Accessory = Content.Load<Texture2D>("accessory-fries")
            };

            // calls component initialize
            base.Initialize();
        }

        internal SpriteBatch spriteBatch = null!;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            spriteBatch.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            // calls FrameworkDispatcher
            base.Update(gameTime);

            Input.Update(IsActive);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(noun.Head, noun.Position, Color.White);
            spriteBatch.Draw(noun.Body, noun.Position, Color.White);
            spriteBatch.Draw(noun.Glasses, noun.Position, Color.White);
            spriteBatch.Draw(noun.Accessory, noun.Position, Color.White);
            spriteBatch.End();

            // calls component draw
            base.Draw(gameTime);
        }
    }
}