using System.IO.Compression;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Random random;
        private Dictionary<string, Rect> rectangles;

        protected override void Initialize()
        {
            random = new Random();

            const string assetDir = "./Content/unzipped";
            Directory.CreateDirectory(assetDir);

            if (Directory.GetFiles(assetDir).Length == 0)
                ZipFile.ExtractToDirectory("./Content/assets_png_sized.zip", assetDir);

            var json = File.ReadAllText(Path.Combine(assetDir, "rectangles.json"));
            rectangles = JsonSerializer.Deserialize<Dictionary<string, Rect>>(json) ?? throw new NullReferenceException("unable to locate rectangle manifest");
            
            noun = GetRandomNoun();

            // calls component initialize
            base.Initialize();
        }

        private Noun GetRandomNoun()
        {
            return new Noun
            {
                Position = new Vector2(graphics.PreferredBackBufferWidth / 2f - 32, graphics.PreferredBackBufferHeight / 2f - 32),
                Body = GetRandomNounPart("body"),
                Head = GetRandomNounPart("head"),
                Glasses = GetRandomNounPart("glasses"),
                Accessory = GetRandomNounPart("accessory"),
                Legs = new NounPart
                {
                    Rectangle = new Rectangle(11, 31, 9, 11),
                    Texture = Content.Load<Texture2D>("legs-default")
                }
            };
        }

        private NounPart GetRandomNounPart(string type)
        {
            var keys = rectangles.Keys.Where(x => x.StartsWith(type)).ToList();
            var key = keys[random.Next(keys.Count)];
            var part = new NounPart
            {
                Rectangle = rectangles[key].ToRectangle(),
                Texture = Content.Load<Texture2D>($"unzipped/{key}")
            };
            return part;
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

            if (Input.KeyWentDown(Keys.Space))
                noun = GetRandomNoun();

            if (Input.KeyWentDown(Keys.Escape))
                Exit();
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