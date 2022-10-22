using Microsoft.Xna.Framework;

namespace NGE.Editor;

public sealed class EditorClient : IEditorClient
{
    private readonly GameWindow window;

    public EditorClient(GameWindow window)
    {
        this.window = window;
    }

    public event EventHandler<EventArgs>? ClientSizeChanged
    {
        add => window.ClientSizeChanged += value;
        remove => window.ClientSizeChanged -= value;
    }
}