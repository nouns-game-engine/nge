using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NGE.Assets;
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

        protected Editor editor = null!;
        private EditorAssetManager editorAssetManager = null!;

        protected void InitializeEditor()
        {
            editor = new Editor(this);

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

            editor.ScanForEditorComponents(configuration, Services);

            bool.TryParse(configuration.GetSection("options")["liveReload"], out var liveReloadEnabled);
            if (liveReloadEnabled)
                AssetRebuild.EnableLiveReload(this);
        }
        
        #region Draw

        protected void DrawEditor(GameTime gameTime)
        {
            DrawMainMenu(gameTime);

            DrawWindows(gameTime);

            if(editor.showDemoWindow)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref editor.showDemoWindow);
            }
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Editor"))
                {
                    if (ImGui.MenuItem("Toggle UI", "F1", editor.editorEnabled, editor.editorEnabled))
                        editor.editorEnabled = !editor.editorEnabled;

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

                foreach (var menu in editor.menus.OrderBy(x => x.GetType().GetCustomAttribute<OrderAttribute>()?.Order ?? 0))
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
                    for (var i = 0; i < editor.windows.Length; i++)
                    {
                        if (!editor.windows[i].Enabled)
                            continue;

                        if (ImGui.MenuItem(editor.windows[i].Label, editor.windows[i].Shortcut, editor.showWindows[i], true))
                            editor.showWindows[i] = !editor.showWindows[i];
                    }

                    ImGui.Separator();

                    if (ImGui.MenuItem("ImGUI Test Window", null, editor.showDemoWindow, true))
                        editor.showDemoWindow = !editor.showDemoWindow;
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
            for (var i = 0; i < editor.showWindows.Length; i++)
            {
                if (!editor.showWindows[i] || !editor.windows[i].Enabled)
                    continue;
                var window = editor.windows[i];
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(window.Width, window.Height), ImGuiCond.FirstUseEver);
                if (ImGui.Begin(window.Label, ref editor.showWindows[i], window.Flags))
                    editor.windows[i].DrawLayout(this, gameTime, ref editor.showWindows[i]);
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
                foreach (var dropHandler in editor.dropHandlers)
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
            for (var i = 0; i < editor.windows.Length; i++)
            {
                if (editor.windows[i] is IEditObject edit && edit.Object == instance)
                    editor.showWindows[i] = !editor.showWindows[i];
            }
        }

        public void EditObject<T>(T instance)
        {
            if (instance == null)
                return;
            if (objects.Contains(instance))
                return;

            objects.Add(instance);
            editor.AddWindow(new ObjectEditingWindow<T>(instance), true);
        }

        public virtual void Reset()
        {
            ClearRetainedObjectEditors();
        }

        private void ClearRetainedObjectEditors()
        {
            for (var i = editor.windows.Length - 1; i >= 0; i--)
            {
                if (editor.windows[i] is not IEditObject)
                    continue;
                Array.Resize(ref editor.windows, editor.windows.Length - 1);
                Array.Resize(ref editor.showWindows, editor.showWindows.Length - 1);
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