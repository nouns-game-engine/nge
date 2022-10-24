using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace NGE.Editor
{
    public interface IEditingContext
    {
        GraphicsDevice GraphicsDevice { get; }
        ICollection<object> ObjectsUnderEdit { get; }
        bool IsActive { get; }
        void EditObject<T>(T instance);
        void ToggleEditorsFor(object instance);
        void Reset();
    }
}
