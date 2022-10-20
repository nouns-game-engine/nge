using ImGuiNET;
using Microsoft.Xna.Framework;
using NGE.Editor;

namespace NGE.Snaps
{
    // ReSharper disable once UnusedMember.Global
    public class ObjectEditingMenu : IEditorMenu
    {
        public bool Enabled => true;
        public string Label => "Objects";

        public void DrawLayout(IEditingContext context, GameTime gameTime)
        {
            if (context.ObjectsUnderEdit.Count == 0)
                return;

            foreach (var item in context.ObjectsUnderEdit)
            {
                if (ImGui.MenuItem(item.GetType().Name))
                    context.ToggleEditorsFor(item);
            }
        }
    }
}