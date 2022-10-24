using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NGE.Core;
using NGE.Core.Configuration;
using NGE.Editor;
using NGE.Screens;
using NGE.Snaps;
using Velentr.Font;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NGE
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
            this.configuration = configuration;
            this.args = args;

            Services.AddService(typeof(IConfiguration), configuration);

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
        private IGame? currentGame;

        protected override void Initialize()
        {
            ProcessCommandLine();

            loadingScreen = new LoadingScreen(this);

#if !WASM
            // registers IEditingContext
            InitializeEditor();
#endif

            Services.AddService(typeof(ContentManager), Content);

            InitializeGame();

            // calls LoadContent
            base.Initialize();
        }

        private void InitializeGame()
        {
            var gameLocation = configuration.GetSection("games")["platformer"];
            var gameAssembly = Assembly.LoadFile(gameLocation);

            var referencesPath = Path.GetDirectoryName(gameLocation)!;
            var referenceFiles = Directory.GetFiles(referencesPath, "*.dll",
                SearchOption.AllDirectories);

            var gameEditors = new Editors();

            AppDomain.CurrentDomain.AssemblyResolve += (_, e) =>
            {
                var fileName = $"{new AssemblyName(e.Name).Name}.dll";
                var assemblyFile = referenceFiles.FirstOrDefault(x => x.EndsWith(fileName));
                if (assemblyFile == null) throw new Exception($"'{fileName}' not found");

                var loadedAssembly = Assembly.LoadFrom(assemblyFile);
                if (editor.excludes.IsExcluded(Path.GetFileName(assemblyFile)))
                    return loadedAssembly;

                editor.InitializeEditorComponents(loadedAssembly, gameEditors, Services);
                editor.InitializeAssetReaders(loadedAssembly);
                return loadedAssembly;
            };

            IGame? game = null;
            foreach (var gameType in gameAssembly.GetTypes().Where(x => typeof(IGame).IsAssignableFrom(x)))
            {
                game = (IGame?) Activator.CreateInstance(gameType, this) ?? (IGame?)Activator.CreateInstance(gameType);
                if (game != null)
                    break;
            }

            if (game == null)
                return;

            Trace.WriteLine($"Found game {game.Name} v{game.Version}");

            foreach (var contentItem in Directory.EnumerateFiles(Path.Combine(referencesPath, "Content")))
            {
                var destFileName = Path.Combine(Content.RootDirectory, Path.GetFileName(contentItem));
                File.Copy(contentItem, destFileName, true);
            }

            currentGame = game;
            currentGame.Initialize(Services);

            if (!string.IsNullOrWhiteSpace(currentGame.Name))
                Window.Title = currentGame.Name;

            editor.AddMenu(new GameMenu(currentGame));

            foreach(var window in gameEditors.windowList)
                editor.AddWindow(window);

            foreach (var menu in gameEditors.menuList)
                editor.AddMenu(menu);

            foreach(var dropHandler in gameEditors.dropHandlerList)
                editor.AddDropHandler(dropHandler);
        }

        internal SpriteBatch sb = null!;
        internal FontManager fontManager = null!;

        protected override void LoadContent()
        {
            if (!assetRebuildQueued)
            {
                sb = new SpriteBatch(GraphicsDevice);
                fontManager = new FontManager(GraphicsDevice);
            }

            if (!oneTimeBackgroundLoad)
            {
                StartBackgroundLoading();
                oneTimeBackgroundLoad = true;
            }

            currentGame?.LoadContent(assetRebuildQueued);
        }

        protected override void UnloadContent()
        {
            currentGame?.UnloadContent(assetRebuildQueued);
            Content.Unload();

            if (!assetRebuildQueued)
            {
                sb.Dispose();
                // fontManager.Dispose(); // bugged 
            }
        }

        protected override void Update(GameTime gameTime)
        {
            // calls FrameworkDispatcher
            base.Update(gameTime);

            Input.Update(IsActive);

            if (DidFinishLoading)
            {

#if !WASM
                if (editor.editorEnabled)
                    editor.UpdateEditor(gameTime);
                else if (Input.KeyWentDown(Keys.F1))
                    editor.editorEnabled = !editor.editorEnabled;

                //
                // Asset Rebuilding:
                if (!IsNetworkGame)
                {
                    if (Input.KeyWentDown(Keys.F5) || assetRebuildQueued)
                    {
                        assetRebuildQueued = true;
                        TryRebuildAssets();
                    }
                }

                currentGame?.Update();

                screenManager.Update();
#endif
                //
                // Reset:
                if (Input.Alt && Input.KeyWentDown(Keys.R))
                {
                    Reset();

                    if (!IsNetworkGame)
                        Trace.TraceError("Reset must provide stable transition to local-only play!");
                }
            }
            else
            {
                if (backgroundLoadTask.IsCompleted)
                {
                    if (!DidFinishLoading)
                        backgroundLoadTask.Wait();
                    OnFinishedLoading();
                }
                else
                {
                    loadingScreen.Update();
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (DidFinishLoading)
            {
#if !WASM
                GraphicsDevice.SetRenderTarget(renderTarget);

                var screenRender = screenManager.Render();
                var screenBounds = Vector2.Zero;

                currentGame?.Draw(renderTarget);
#endif

#if !WASM
                GraphicsDevice.SetRenderTarget(null);

                sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                sb.Draw(renderTarget, Vector2.Zero, Color.White);
                sb.End();

                if (screenRender != null)
                {
                    sb.Begin(0, null, SamplerState.PointClamp, null, null);
                    sb.Draw(screenRender, new Vector2(screenBounds.X, screenBounds.Y), null, Color.White, 0, Vector2.Zero, 1, 0, 0);
                    sb.End();
                }

                if (editor.editorEnabled)
                {
                    imGui.BeforeLayout(gameTime);
                    DrawEditor(gameTime);
                    imGui.AfterLayout();
                }
#endif
            }
            else
            {
                loadingScreen.Draw(gameTime);
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
            var backgroundTasks = currentGame?.StartBackgroundLoading() ?? Array.Empty<Task>();

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
                Trace.WriteLine($"Background load completed, took: {sw.Elapsed}");
                OnFinishedLoading();
            });
#endif
        }

        private ScreenContext screenContext = null!;
        private ScreenManager screenManager = null!;

        public void OnFinishedLoading()
        {
            DidFinishLoading = true;
            TargetElapsedTime = Constants.defaultFrameTime;
            currentGame?.OnFinishedBackgroundLoading(sb);

            screenContext = new ScreenContext();
            screenManager = new ScreenManager(this, screenContext);
        }

        #endregion

        #region Command Line

        public void ProcessCommandLine()
        {
            CommandLine.ProcessArguments(ref configuration, args);
        }

#endregion

        public override void Reset()
        {
            currentGame?.Reset();
            base.Reset();
        }

        #region Uptime

        private readonly DateTime startedAt = DateTime.UtcNow;
        internal TimeSpan RunningFor => DateTime.UtcNow - startedAt;

        #endregion
    }
}