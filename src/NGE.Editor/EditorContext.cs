using System;

namespace NGE.Editor;

public sealed class EditorContext
{
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
}