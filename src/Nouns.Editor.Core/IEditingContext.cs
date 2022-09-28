using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Nouns.Editor
{
    public interface IEditingContext
    {
        GraphicsDevice GraphicsDevice { get; }
        ICollection<object> Objects { get; }
        void EditObject<T>(T instance);
        void ToggleEditorsFor(object item);
        void Reset();
    }
}
