using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nouns.Core;
using Nouns.Core.Configuration;
using Nouns.Assets.Core;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Nouns.Snaps;

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
                if (assemblyFile != null)
                {
                    var loadedAssembly = Assembly.LoadFrom(assemblyFile);
                    InitializeEditorComponents(loadedAssembly, gameEditors);
                    return loadedAssembly;
                }

                throw new Exception($"'{fileName}' not found");
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

            foreach (var contentItem in Directory.EnumerateFiles(Path.Combine(referencesPath, "Content")))
            {
                var destFileName = Path.Combine(Content.RootDirectory, Path.GetFileName(contentItem));
                File.Copy(contentItem, destFileName, true);
            }

            currentGame = game;
            currentGame.Initialize(Services);

            if (!string.IsNullOrWhiteSpace(currentGame.Name))
                Window.Title = currentGame.Name;

            AddMenu(new GameMenu(currentGame));

            foreach(var window in gameEditors.windowList)
                AddWindow(window);

            foreach (var menu in gameEditors.menuList)
                AddMenu(menu);

            foreach(var dropHandler in gameEditors.dropHandlerList)
                AddDropHandler(dropHandler);
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
            
            // for testing only, should be removed
            AssetReader.Add<Texture2D>(".png", (fullPath, _, services) => Texture2D.FromStream(services.GraphicsDevice(), File.OpenRead(fullPath)));
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

                currentGame?.Update();
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
                currentGame?.Draw(renderTarget);
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

        public void OnFinishedLoading()
        {
            DidFinishLoading = true;
            TargetElapsedTime = Constants.defaultFrameTime;
            currentGame?.OnFinishedBackgroundLoading(sb);
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
    }
}