namespace NGE.Editor;

public sealed class EditorContext
{
    public IEditorWindow[] windows = null!;
    public IEditorMenu[] menus = null!;
    public IEditorDropHandler[] dropHandlers = null!;

    public bool[] showWindows = null!;
    public bool showDemoWindow;
    public bool devMenuEnabled = true;

}