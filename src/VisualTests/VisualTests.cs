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
                Position = new Vector2(graphics.PreferredBackBufferWidth / 2f - 32, graphics.PreferredBackBufferHeight / 2f - 32),
                Body = new NounPart
                {
                    Rectangle = new Rectangle(9, 21, 14,11),
                    Texture = Content.Load<Texture2D>("body-computerblue")
                },
                Head = new NounPart
                {
                    Rectangle = new Rectangle(6, 3, 24, 18),
                    Texture = Content.Load<Texture2D>("head-aardvark"),
                },
                Glasses = new NounPart
                {
                    Rectangle = new Rectangle(7, 11, 16, 6),
                    Texture = Content.Load<Texture2D>("glasses-hip-rose")
                },
                Accessory = new NounPart
                {
                    Rectangle = new Rectangle(14, 23, 5, 7),
                    Texture = Content.Load<Texture2D>("accessory-fries")
                },
                Legs = new NounPart
                {
                    Rectangle = new Rectangle(13, 32, 9, 11),
                    Texture = Content.Load<Texture2D>("legs-default")
                }
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
            noun.Draw(spriteBatch);
            spriteBatch.End();

            // calls component draw
            base.Draw(gameTime);
        }
    }
}