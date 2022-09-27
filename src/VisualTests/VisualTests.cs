using System.IO.Compression;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core;
using Nouns.Pipeline;

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
            const string assetDir = "./Content/unzipped";
            Directory.CreateDirectory(assetDir);

            if (Directory.GetFiles(assetDir).Length == 0)
                ZipFile.ExtractToDirectory("./Content/assets_png_sized.zip", assetDir);

            var json = File.ReadAllText(Path.Combine(assetDir, "rectangles.json"));
            var rectangles = JsonSerializer.Deserialize<Dictionary<string, Rect>>(json);
            
            noun = new Noun
            {
                Position = new Vector2(graphics.PreferredBackBufferWidth / 2f - 32, graphics.PreferredBackBufferHeight / 2f - 32),
                Body = new NounPart
                {
                    Rectangle = rectangles["body-computerblue"].ToRectangle(),
                    Texture = Content.Load<Texture2D>("unzipped/body-computerblue")
                },
                Head = new NounPart
                {
                    Rectangle = rectangles["head-aardvark"].ToRectangle(),
                    Texture = Content.Load<Texture2D>("unzipped/head-aardvark"),
                },
                Glasses = new NounPart
                {
                    Rectangle = rectangles["glasses-square-blue"].ToRectangle(),
                    Texture = Content.Load<Texture2D>("unzipped/glasses-square-blue")
                },
                Accessory = new NounPart
                {
                    Rectangle = rectangles["accessory-fries"].ToRectangle(),
                    Texture = Content.Load<Texture2D>("unzipped/accessory-fries")
                },
                Legs = new NounPart
                {
                    Rectangle = new Rectangle(11, 32, 9, 11),
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

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            noun.Draw(spriteBatch, 3);
            spriteBatch.End();

            // calls component draw
            base.Draw(gameTime);
        }
    }
}