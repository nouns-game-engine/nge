using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NGE.Assets;
using NGE.Core;
using NGE.Snaps;
using SDL2;

namespace NGE.Editor
{
    // ReSharper disable once UnusedMember.Global
    [Obsolete("Remove this base class hierarchy so the editor is portable, and opt-in")]
    public abstract class EditableGame : Game, IEditingContext, IAssetRebuildReceiver
    {
        protected IConfiguration configuration = null!;

        
        protected ImGuiRenderer imGui = null!;
        protected RenderTarget2D renderTarget = null!;

        protected EditorContext context;
        private EditorAssetManager editorAssetManager = null!;

        protected void InitializeEditor()
        {
            context = new EditorContext();

            editorAssetManager = new EditorAssetManager(Services);

            Services.AddService(typeof(EditorAssetManager), editorAssetManager);

            SDL.SDL_AddEventWatch(dropFileEvent = DropFileEvent, IntPtr.Zero);

            imGui = new ImGuiRenderer(this);
            imGui.RebuildFontAtlas();

            Services.AddService(typeof(ImGuiRenderer), imGui);

            CreateRenderTarget();

            Window.ClientSizeChanged += delegate { CreateRenderTarget(); };

            void CreateRenderTarget()
            {
                renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                    false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24,
                    0, RenderTargetUsage.PreserveContents);
            }

            context.ScanForEditorComponents(configuration, Services);

            bool.TryParse(configuration.GetSection("options")["liveReload"], out var liveReloadEnabled);
            if (liveReloadEnabled)
                AssetRebuild.EnableLiveReload(this);
        }


        #region Update

        private bool lastActive;

        protected void UpdateEditor(GameTime gameTime)
        {
            if (lastActive != IsActive)
                Trace.TraceInformation(IsActive ? "editor gained focus" : "editor lost focus");

            lastActive = IsActive;

            //
            // UI toggle:
            if (Input.KeyWentDown(Keys.F1))
                context.devMenuEnabled = !context.devMenuEnabled;

            //
            // Reset:
            if (Input.Alt && Input.KeyWentDown(Keys.R))
            {
                Reset();

                if(!IsNetworkGame)
                    Trace.TraceError("Reset must provide stable transition to local-only play!");
            }

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

            //
            // Windows:
            for (var i = 0; i < context.windows.Length; i++)
            {
                var window = context.windows[i];
                if (window.Shortcut == null)
                    continue;

                var tokens = window.Shortcut.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                for (var j = 0; j < tokens.Length; j++)
                    tokens[j] = tokens[j].Trim();

                var index = i;
                bool isControl = false, isAlt = false, isShift = false;

                for (var t = 0; t < tokens.Length; t++)
                {
                    if (tokens[t].Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                    {
                        isControl = true;
                        continue;
                    }
                    if (tokens[t].Equals("Alt", StringComparison.OrdinalIgnoreCase))
                    {
                        isAlt = true;
                        continue;
                    }
                    if (tokens[t].Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        isShift = true;
                        continue;
                    }

                    if (tokens[t].Equals("0", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D0";
                    }
                    if (tokens[t].Equals("1", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D1";
                    }
                    if (tokens[t].Equals("2", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D2";
                    }
                    if (tokens[t].Equals("3", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D3";
                    }
                    if (tokens[t].Equals("4", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D4";
                    }
                    if (tokens[t].Equals("5", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D5";
                    }
                    if (tokens[t].Equals("6", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D6";
                    }
                    if (tokens[t].Equals("7", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D7";
                    }
                    if (tokens[t].Equals("8", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D8";
                    }
                    if (tokens[t].Equals("9", StringComparison.OrdinalIgnoreCase))
                    {
                        tokens[t] = "D9";
                    }

                    if (!Enum.TryParse(tokens[t], true, out Keys keys))
                        continue;

                    var control = isControl;
                    var alt = isAlt;
                    var shift = isShift;

                    if (control && !Input.Control)
                        continue;
                    if (alt && !Input.Alt)
                        continue;
                    if (shift && !Input.Shift)
                        continue;

                    if (Input.KeyWentDown(keys))
                        context.showWindows[index] = !context.showWindows[index];
                }
            }

            foreach(var menu in context.menus)
                menu.UpdateLayout(this, gameTime);

            foreach (var window in context.windows)
                window.UpdateLayout(this, gameTime);
        }

        #endregion

        #region Draw

        protected void DrawEditor(GameTime gameTime)
        {
            DrawMainMenu(gameTime);

            DrawWindows(gameTime);

            if(context.showDemoWindow)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref context.showDemoWindow);
            }
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Editor"))
                {
                    if (ImGui.MenuItem("Toggle UI", "F1", context.devMenuEnabled, context.devMenuEnabled))
                        context.devMenuEnabled = !context.devMenuEnabled;

                    if (ImGui.MenuItem("Reset Game", "Alt+R"))
                        Reset();

                    var options = configuration.GetSection("options");
                    bool.TryParse(options["liveReload"], out var liveReload);
                    if (ImGui.Checkbox("Live Reload", ref liveReload))
                    {
                        options["liveReload"] = liveReload.ToString();

                        if (liveReload)
                            AssetRebuild.EnableLiveReload(this);
                        else
                            AssetRebuild.DisableLiveReload();
                    }

                    if (!IsNetworkGame)
                    {
                        if (ImGui.MenuItem("Rebuild Assets", "F5"))
                        {
                            assetRebuildQueued = true;
                            TryRebuildAssets();
                        }
                    }

                    if (ImGui.MenuItem("Quit"))
                        Exit();

                    ImGui.EndMenu();
                }

                foreach (var menu in context.menus.OrderBy(x => x.GetType().GetCustomAttribute<OrderAttribute>()?.Order ?? 0))
                {
                    if (menu is ObjectEditingMenu && objects.Count == 0)
                        continue;

                    if (ImGui.BeginMenu(menu.Label, menu.Enabled))
                    {
                        menu.DrawLayout(this, gameTime);
                        ImGui.EndMenu();
                    }
                }

                if (ImGui.BeginMenu("Window"))
                {
                    for (var i = 0; i < context.windows.Length; i++)
                    {
                        if (!context.windows[i].Enabled)
                            continue;

                        if (ImGui.MenuItem(context.windows[i].Label, context.windows[i].Shortcut, context.showWindows[i], true))
                            context.showWindows[i] = !context.showWindows[i];
                    }

                    ImGui.Separator();

                    if (ImGui.MenuItem("ImGUI Test Window", null, context.showDemoWindow, true))
                        context.showDemoWindow = !context.showDemoWindow;
                    ImGui.EndMenu();
                }

                var fps = $"{ImGui.GetIO().Framerate:F2} FPS ({1000f / ImGui.GetIO().Framerate:F2} ms)";
                ImGui.SameLine(Window.ClientBounds.Width - ImGui.CalcTextSize(fps).X - 10f);
                if (gameTime.IsRunningSlowly)
                    ImGui.TextColored(Color.Red.ToImGuiVector4(), fps);
                else
                    ImGui.Text(fps);

                ImGui.EndMainMenuBar();
            }
        }

        private void DrawWindows(GameTime gameTime)
        {
            for (var i = 0; i < context.showWindows.Length; i++)
            {
                if (!context.showWindows[i] || !context.windows[i].Enabled)
                    continue;
                var window = context.windows[i];
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(window.Width, window.Height), ImGuiCond.FirstUseEver);
                if (ImGui.Begin(window.Label, ref context.showWindows[i], window.Flags))
                    context.windows[i].DrawLayout(this, gameTime, ref context.showWindows[i]);
                ImGui.End();
            }
        }

        #endregion

        #region Drop Handling

        // ReSharper disable once NotAccessedField.Local
        private SDL.SDL_EventFilter dropFileEvent = null!;

        // See: https://wiki.libsdl.org/SDL_SetEventFilter
        private int DropFileEvent(IntPtr func, IntPtr evtPtr)
        {
            try
            {
                var evt = (SDL.SDL_Event)(Marshal.PtrToStructure(evtPtr, typeof(SDL.SDL_Event)) ?? throw new NullReferenceException());
                if (evt.type != SDL.SDL_EventType.SDL_DROPFILE)
                    return 0;

                var filename = SDL.UTF8_ToManaged(evt.drop.file, true);
                Trace.WriteLine($"File dropped: {filename}");
                foreach (var dropHandler in context.dropHandlers)
                {
                    if (dropHandler.Enabled && dropHandler.Handle(this, filename))
                    {
                        break;
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region IEditingContext

        private readonly List<object> objects = new();

        public ICollection<object> ObjectsUnderEdit => objects;

        public void ToggleEditorsFor(object instance)
        {
            for (var i = 0; i < context.windows.Length; i++)
            {
                if (context.windows[i] is IEditObject edit && edit.Object == instance)
                    context.showWindows[i] = !context.showWindows[i];
            }
        }

        public void EditObject<T>(T instance)
        {
            if (instance == null)
                return;
            if (objects.Contains(instance))
                return;

            objects.Add(instance);
            context.AddWindow(new ObjectEditingWindow<T>(instance), true);
        }

        public virtual void Reset()
        {
            ClearRetainedObjectEditors();
        }

        private void ClearRetainedObjectEditors()
        {
            for (var i = context.windows.Length - 1; i >= 0; i--)
            {
                if (context.windows[i] is not IEditObject)
                    continue;
                Array.Resize(ref context.windows, context.windows.Length - 1);
                Array.Resize(ref context.showWindows, context.showWindows.Length - 1);
            }

            ObjectsUnderEdit.Clear();
        }

        #endregion

        #region Asset Reload

        public bool assetRebuildQueued;

        public void ShouldRebuildAssets()
        {
            assetRebuildQueued = true;
        }

        public void TryRebuildAssets()
        {
            try
            {
                // Remove any retained editor objects
                // This is likely a design smell and generic object editing should be tossed?
                ClearRetainedObjectEditors();

                if (!AssetRebuild.Run())
                    return;

                UnloadContent();
                LoadContent();
            }
            finally
            {
                assetRebuildQueued = false;
            }
        }

        #endregion

        #region Networking

        public virtual bool IsNetworkGame => false;

        #endregion
    }
}