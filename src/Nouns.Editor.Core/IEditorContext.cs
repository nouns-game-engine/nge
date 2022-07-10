using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nouns.Editor
{
    public interface IEditorContext
    {
        GraphicsDevice GraphicsDevice { get; }
        ICollection<object> Objects { get; }
        void ToggleEditorsFor(object item);
        void Reset();
    }
}
