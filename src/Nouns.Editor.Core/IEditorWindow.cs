using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Nouns.Editor
{
    public interface IEditorWindow : IEditorEnabled
    {
        ImGuiWindowFlags Flags { get; }
        string? Label { get; } 
        string? Shortcut { get; }
        int Width { get; }
        int Height { get; }
        void Layout(IEditingContext context, GameTime gameTime, ref bool opened);
    }
}