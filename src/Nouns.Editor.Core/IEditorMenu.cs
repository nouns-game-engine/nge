using Microsoft.Xna.Framework;

namespace Nouns.Editor;

public interface IEditorMenu : IEditorEnabled
{
    string Label { get; }
    void Layout(IEditorContext context, GameTime gameTime);
}