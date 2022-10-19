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

        void UpdateLayout(IEditingContext context, GameTime gameTime) { }
        void DrawLayout(IEditingContext context, GameTime gameTime, ref bool opened);
    }
}