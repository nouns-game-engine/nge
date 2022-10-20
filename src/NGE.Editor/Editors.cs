using System.Collections.Generic;

namespace NGE.Editor
{
    public sealed class Editors
    {
        public List<IEditorWindow>  windowList = new();
        public List<IEditorMenu> menuList = new();
        public List<IEditorDropHandler> dropHandlerList = new();
    }
}
