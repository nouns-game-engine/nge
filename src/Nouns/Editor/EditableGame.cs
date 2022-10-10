using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nouns.Assets;
using Nouns.Assets.Core;
using Nouns.Core;
using Nouns.Snaps;
using SDL2;

namespace Nouns.Editor
{
    public abstract class EditableGame : Game, IEditingContext
    {
        protected IConfiguration configuration = null!;

        protected IEditorWindow[] windows = null!;
        protected IEditorMenu[] menus = null!;
        protected IEditorDropHandler[] dropHandlers = null!;

        protected bool[] showWindows = null!;
        private bool showDemoWindow;
        protected bool devMenuEnabled = true;

        protected ImGuiRenderer imGui = null!;
        protected RenderTarget2D renderTarget = null!;

        protected void ImGuiInit()
        {
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
                devMenuEnabled = !devMenuEnabled;

            //
            // Reset:
            if (Input.Alt && Input.KeyWentDown(Keys.R))
                Reset();

            //
            // Windows:
            for (var i = 0; i < windows.Length; i++)
            {
                var window = windows[i];
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
                        return;
                    if (alt && !Input.Alt)
                        return;
                    if (shift && !Input.Shift)
                        return;

                    if (Input.KeyWentDown(keys))
                        showWindows[index] = !showWindows[index];
                }
            }
        }

        #endregion

        #region Draw

        protected void DrawEditor(GameTime gameTime)
        {
            DrawMainMenu(gameTime);

            DrawWindows(gameTime);

            if (showDemoWindow)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref showDemoWindow);
            }
        }

        private void DrawMainMenu(GameTime gameTime)
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("Editor"))
                {
                    if (ImGui.MenuItem("Toggle UI", "F1", devMenuEnabled, devMenuEnabled))
                        devMenuEnabled = !devMenuEnabled;

                    if (ImGui.MenuItem("Reset Game", "Alt+R"))
                        Reset();

                    if (ImGui.MenuItem("Quit"))
                        Exit();

                    ImGui.EndMenu();
                }

                foreach (var menu in menus.OrderBy(x => x.GetType().GetCustomAttribute<OrderAttribute>()?.Order ?? 0))
                {
                    if (menu is ObjectEditingMenu && objects.Count == 0)
                        continue;

                    if (ImGui.BeginMenu(menu.Label, menu.Enabled))
                    {
                        menu.Layout(this, gameTime);
                        ImGui.EndMenu();
                    }
                }

                if (ImGui.BeginMenu("Window"))
                {
                    for (var i = 0; i < windows.Length; i++)
                    {
                        if (!windows[i].Enabled)
                            continue;

                        if (ImGui.MenuItem(windows[i].Label, windows[i].Shortcut, showWindows[i], true))
                            showWindows[i] = !showWindows[i];
                    }

                    ImGui.Separator();

                    if (ImGui.MenuItem("ImGUI Test Window", null, showDemoWindow, true))
                        showDemoWindow = !showDemoWindow;
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
            for (var i = 0; i < showWindows.Length; i++)
            {
                if (!showWindows[i] || !windows[i].Enabled)
                    continue;
                var window = windows[i];
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(window.Width, window.Height), ImGuiCond.FirstUseEver);
                if (ImGui.Begin(window.Label, ref showWindows[i], window.Flags))
                    windows[i].Layout(this, gameTime, ref showWindows[i]);
                ImGui.End();
            }
        }

        #endregion

        private EditorAssetManager editorAssetManager = null!;

        protected void InitializeEditor()
        {
            editorAssetManager = new EditorAssetManager(Content.ServiceProvider);

            Services.AddService(typeof(EditorAssetManager), editorAssetManager);

            SDL.SDL_AddEventWatch(dropFileEvent = DropFileEvent, IntPtr.Zero);

            ImGuiInit();

            ScanForEditorComponents();
        }

        private void ScanForEditorComponents()
        {
            var editors = new Editors();

            var location = Assembly.GetExecutingAssembly().Location;
            var binDir = Path.GetDirectoryName(location) ?? location;

            var self = Assembly.GetExecutingAssembly();

            InitializeEditorComponents(self, editors);

            var visited = new HashSet<string> { self.Location };

            var loaded = AppDomain.CurrentDomain.GetAssemblies()
                .ToDictionary(k => k.Location, v => v);
            
            foreach (var dll in Directory.GetFiles(binDir, "*.dll"))
            {
                if (!File.Exists(dll))
                    continue;

                try
                {
                    if(!loaded.TryGetValue(dll, out var assembly))
                        assembly = Assembly.LoadFile(dll);

                    if (visited.Contains(assembly.Location))
                        continue;

                    visited.Add(assembly.Location);

                    InitializeEditorComponents(assembly, editors);
                }
                catch (Exception ex)
                {
                    Trace.TraceError($"{ex}");
                }
            }

            //
            // Windows:
            editors.windowList.Sort(OrderExtensions.TrySortByOrder);
            windows = editors.windowList.ToArray();
            showWindows = new bool[windows.Length];

            // 
            // Menus:
            editors.menuList.Sort(OrderExtensions.TrySortByOrder);
            menus = editors.menuList.ToArray();

            //
            // Drops:
            editors.dropHandlerList.Sort(OrderExtensions.TrySortByOrder);
            dropHandlers = editors.dropHandlerList.ToArray();
        }

        private void InitializeEditorComponents(Assembly assembly, Editors editors)
        {
            foreach (var type in assembly.GetEditorTypes())
            {
                // ctor(IServiceProvider)
                if (ActivateWithConstructor(new[] {typeof(IServiceProvider)}, editors, type, Services))
                    continue;

                // ctor()
                if (ActivateWithConstructor(Type.EmptyTypes, editors, type))
                    continue;

                if (type.Implements<IEditObject>())
                    continue; // deferred

                Trace.TraceError($"Editor component '{type.Name}' has no valid constructors");
            }
        }

        private static bool ActivateWithConstructor(Type[] parameterTypes, Editors editors, Type type, params object[] parameters)
        {
            var ctor = type.GetConstructor(parameterTypes);
            if (ctor == null)
                return false;

            if (type.Implements<IEditorWindow>() && Activator.CreateInstance(type, parameters) is IEditorWindow window)
            {
                editors.windowList.Add(window);
                Trace.TraceInformation($"Adding window '{type.Name}'");
                return true;
            }

            if (type.Implements<IEditorMenu>() && Activator.CreateInstance(type, parameters) is IEditorMenu menu)
            {
                editors.menuList.Add(menu);
                Trace.TraceInformation($"Adding menu '{type.Name}'");
                return true;
            }

            if (type.Implements<IEditorDropHandler>() &&
                Activator.CreateInstance(type, parameters) is IEditorDropHandler dropHandler)
            {
                editors.dropHandlerList.Add(dropHandler);
                Trace.TraceInformation($"Adding drop handler '{type.Name}'");
                return true;
            }

            return false;
        }

        // ReSharper disable once UnusedMember.Global
        public void AddWindow(IEditorWindow window, bool isVisible = true)
        {
            Array.Resize(ref windows, windows.Length + 1);
            Array.Resize(ref showWindows, showWindows.Length + 1);
            windows[^1] = window;
            showWindows[windows.Length - 1] = isVisible;
        }

        // ReSharper disable once UnusedMember.Global
        public void AddMenu(IEditorMenu menu)
        {
            Array.Resize(ref menus, menus.Length + 1);
            menus[^1] = menu;
        }

        // ReSharper disable once UnusedMember.Global
        public void AddDropHandler(IEditorDropHandler dropHandler)
        {
            Array.Resize(ref dropHandlers, dropHandlers.Length + 1);
            dropHandlers[^1] = dropHandler;
        }

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
                foreach (var dropHandler in dropHandlers)
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

        public ICollection<object> Objects => objects;

        public void ToggleEditorsFor(object instance)
        {
            for (var i = 0; i < windows.Length; i++)
            {
                if (windows[i] is IEditObject edit && edit.Object == instance)
                    showWindows[i] = !showWindows[i];
            }
        }

        public void EditObject<T>(T instance)
        {
            if (instance == null)
                return;
            if (objects.Contains(instance))
                return;

            objects.Add(instance);
            AddWindow(new ObjectEditingWindow<T>(instance));
        }

        public virtual void Reset()
        {
            for (var i = windows.Length - 1; i >= 0; i--)
            {
                if (windows[i] is not IEditObject)
                    continue;
                Array.Resize(ref windows, windows.Length - 1);
                Array.Resize(ref showWindows, showWindows.Length - 1);
            }
        }

        #endregion
    }
}