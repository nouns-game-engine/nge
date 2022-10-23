using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NGE.Core;
using NGE.Core.Assets;

namespace NGE.Editor;

public sealed class EditorContext
{
    #region Snaps

    public IEditorWindow[] windows = null!;
    public IEditorMenu[] menus = null!;
    public IEditorDropHandler[] dropHandlers = null!;

    public bool[] showWindows = null!;
    public bool showDemoWindow;
    public bool devMenuEnabled = true;

    // ReSharper disable once UnusedMember.Global
    public void AddWindow(IEditorWindow window, bool isVisible = false)
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

    #endregion

    #region Scanning

    public EditorExcludes excludes = null!;

    public void ScanForEditorComponents(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        var editors = new Editors();

        var location = Assembly.GetExecutingAssembly().Location;
        var binDir = Path.GetDirectoryName(location) ?? location;

        var self = Assembly.GetExecutingAssembly();

        InitializeEditorComponents(self, editors, serviceProvider);
        InitializeAssetReaders(self);

        var visited = new HashSet<string> { self.Location };

        excludes = EditorExcludes.FromConfiguration(configuration);

        var loaded = AppDomain.CurrentDomain.GetAssemblies()
            .ToDictionary(k => k.Location, v => v);

        foreach (var dll in Directory.GetFiles(binDir, "*.dll"))
        {
            if (!File.Exists(dll))
                continue;

            var dllName = Path.GetFileNameWithoutExtension(dll);

            if (excludes.IsExcluded(dllName))
            {
                visited.Add(dll);
                continue;
            }

            try
            {
                if (!loaded.TryGetValue(dll, out var assembly))
                    assembly = Assembly.LoadFile(dll);

                if (visited.Contains(assembly.Location))
                    continue;

                visited.Add(assembly.Location);

                InitializeEditorComponents(assembly, editors, serviceProvider);
                InitializeAssetReaders(assembly);
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

    public void InitializeEditorComponents(Assembly assembly, Editors editors, IServiceProvider serviceProvider)
    {
        foreach (var type in assembly.GetEditorTypes())
        {
            // ctor(IServiceProvider)
            if (ActivateWithConstructor(new[] { typeof(IServiceProvider) }, editors, type, serviceProvider))
                continue;

            // ctor()
            if (ActivateWithConstructor(Type.EmptyTypes, editors, type))
                continue;

            if (type.Implements<IEditObject>())
                continue; // deferred

            Trace.TraceError($"Editor component '{type.Name}' has no valid constructors");
        }
    }

    public void InitializeAssetReaders(Assembly assembly)
    {
        foreach (var type in assembly.GetAssetReaderTypes())
        {
            // ctor()
            if (Activator.CreateInstance(type) is IAssetReader reader)
            {
                reader.Load();
                continue;
            }

            Trace.TraceError($"Asset reader '{type.Name}' has no valid constructors");
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

    #endregion
}