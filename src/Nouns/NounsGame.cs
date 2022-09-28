using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nouns.Assets.MagicaVoxel;
using Nouns.Core;
using Platformer;

#if !WASM
using Nouns.Editor;
#endif

namespace Nouns
{
    internal class NounsGame :
#if !WASM
    EditableGame
#else
    Game
#endif
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable (retained)
        private readonly GraphicsDeviceManager graphics;
        private readonly string[] args;

        public NounsGame(IConfiguration configuration, params string[] args)
        {
#if !WASM
            this.configuration = configuration;
#endif
            this.args = args;

            TargetElapsedTime = Constants.loadingScreenFrameTime;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            Content.RootDirectory = "Content";

            Window.Title = "nounsgame.wtf";
            Window.AllowUserResizing = true;
            IsMouseVisible = true;
        }

        private LoadingScreen loadingScreen = null!;
        private PlatformerGame game = null!;

        protected override void Initialize()
        {
            ProcessCommandLine();

            loadingScreen = new LoadingScreen(this);

#if !WASM
            InitializeEditor(Content.RootDirectory);
#endif

            game = new PlatformerGame(Content, this);
            game.Initialize();
            
            // calls LoadContent
            base.Initialize();
        }

        internal SpriteBatch sb = null!;

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);

            if (!oneTimeBackgroundLoad)
            {
                StartBackgroundLoading();
                oneTimeBackgroundLoad = true;
            }

            VoxReader.Initialize();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            sb.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            // calls FrameworkDispatcher
            base.Update(gameTime);

            Input.Update(IsActive);

            if (DidFinishLoading)
            {

#if !WASM
                if (devMenuEnabled)
                    UpdateEditor(gameTime);

                else if (Input.KeyWentDown(Keys.F1))
                    devMenuEnabled = !devMenuEnabled;

                game.Update();
#endif
            }
            else
            {
                if (backgroundLoadTask.IsCompleted)
                {
                    if (!DidFinishLoading)
                        backgroundLoadTask.Wait();
                    OnFinishedLoading();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (DidFinishLoading)
            {
#if !WASM
                GraphicsDevice.SetRenderTarget(renderTarget);
                game.Draw(renderTarget);
#endif

#if !WASM
                GraphicsDevice.SetRenderTarget(null);

                sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                sb.Draw(renderTarget, Vector2.Zero, Color.White);
                sb.End();

                if (devMenuEnabled)
                {
                    imGui.BeforeLayout(gameTime);
                    DrawEditor(gameTime);
                    imGui.AfterLayout();
                }
#endif
            }
            else
            {
                loadingScreen.Draw();
            }

            // calls component draw
            base.Draw(gameTime);
        }

        #region Background Loading 

        private bool oneTimeBackgroundLoad;

        private Task backgroundLoadTask = null!;

        public bool DidFinishLoading { get; private set; }

        private void StartBackgroundLoading()
        {
#if !WASM
            var backgroundTasks = game.StartBackgroundLoading();

            if (backgroundTasks.Length == 0)
            {
                DidFinishLoading = true;
                OnFinishedLoading();
                return;
            }

            var sw = Stopwatch.StartNew();

            backgroundLoadTask = Task.Factory.ContinueWhenAll(backgroundTasks, tasks =>
            {
                Task.WaitAll(tasks);
                Trace.WriteLine($"background load completed, took: {sw.Elapsed}");
                OnFinishedLoading();
            });
#else
            game.BackgroundLoad();
#endif
        }

        public void OnFinishedLoading()
        {
            DidFinishLoading = true;
            TargetElapsedTime = Constants.defaultFrameTime;
            game.OnFinishedLoading(sb);
        }

#endregion

#region Command Line

        public void ProcessCommandLine()
        {
            var arguments = new Queue<string>(args);

            while (arguments.Count > 0)
            {
                string arg = arguments.Dequeue();

                switch (arg.ToLowerInvariant())
                {
                    default:
                        break;
                }
            }
        }

        static bool EndOfSubArguments(Queue<string> arguments)
        {
            // + is to support steam-style arguments
            return arguments.Count == 0 || arguments.Peek().StartsWith("--") || arguments.Peek().StartsWith("+");
        }

#endregion
    }
}