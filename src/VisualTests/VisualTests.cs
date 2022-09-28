using System.IO.Compression;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nouns.Core;
using Nouns.Pipeline;
using Velentr.Font;
using Text = Velentr.Font.Text;

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

        private Random random;
        private Dictionary<string, Rect> rectangles;
        private Noun noun;

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
                    Name = "default",
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
                Name = key.Replace($"{type}-", "").Replace("-", " "),
                Rectangle = rectangles[key].ToRectangle(),
                Texture = Content.Load<Texture2D>($"unzipped/{key}")
            };
            return part;
        }

        internal SpriteBatch spriteBatch = null!;
        
        private FontManager fontManager;
        private Text chooseYourHero;
        private Font titleFont;

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            fontManager = new FontManager(GraphicsDevice);
            titleFont = fontManager.GetFont("./Content/LondrinaSolid-Regular.ttf", 48);
            chooseYourHero = titleFont.MakeText("Choose Your Hero!");
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

            var font = fontManager.GetFont("./Content/nountown.otf", 48);

            spriteBatch.Begin();
            spriteBatch.DrawString(chooseYourHero, new Vector2(50, 50), Color.Red);
            spriteBatch.DrawString(font, "Head " + noun.Head.Name, new Vector2(50, 98), Color.White);
            spriteBatch.DrawString(font, "Glasses " + noun.Glasses.Name, new Vector2(50, 146), Color.White);
            spriteBatch.DrawString(font, "Body " + noun.Body.Name, new Vector2(50, 194), Color.White);
            spriteBatch.DrawString(font, "Accessory " + noun.Accessory.Name, new Vector2(50, 242), Color.White);
            spriteBatch.DrawString(font, "Legs " + noun.Legs.Name, new Vector2(50, 290), Color.White);
            spriteBatch.End();

            // calls component draw
            base.Draw(gameTime);
        }
    }
}