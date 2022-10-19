using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Editor;

namespace NGE.Snaps
{
    // ReSharper disable once UnusedMember.Global
    public class ObjectEditingMenu : IEditorMenu
    {
        public bool Enabled => true;
        public string Label => "Objects";

        public void Layout(IEditingContext context, GameTime gameTime)
        {
            if (context.Objects.Count == 0)
                return;

            foreach (var item in context.Objects)
            {
                if (ImGui.MenuItem(item.GetType().Name))
                    context.ToggleEditorsFor(item);
            }
        }
    }
}