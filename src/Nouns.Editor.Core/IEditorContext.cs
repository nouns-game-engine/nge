using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Editor;

public interface IEditorContext
{
    GraphicsDevice GraphicsDevice { get; }
    ICollection<object> Objects { get; }
    void ToggleEditorsFor(object item);
    void Reset();
}