using ImGuiNET;
using Microsoft.Xna.Framework;
using Nouns.Editor;

namespace Nouns.Snaps;

public class ObjectEditingMenu : IEditorMenu
{
    public bool Enabled => true;
    public string Label => "Objects";

    public void Layout(IEditorContext context, GameTime gameTime)
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