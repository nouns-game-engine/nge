using System;

namespace NGE.Editor;

public interface IEditorClient
{
    event EventHandler<EventArgs>? ClientSizeChanged;
}