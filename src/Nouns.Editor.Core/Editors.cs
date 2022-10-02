using System.Collections.Generic;

namespace Nouns.Editor
{
    public sealed class Editors
    {
        public List<IEditorWindow>  windowList = new();
        public List<IEditorMenu> menuList = new();
        public List<IEditorDropHandler> dropHandlerList = new();
    }
}
