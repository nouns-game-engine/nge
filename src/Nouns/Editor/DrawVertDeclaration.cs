using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Editor
{
    public static class DrawVertDeclaration
    {
        public static readonly VertexDeclaration declaration;

        public static readonly int size;

        static DrawVertDeclaration()
        {
            unsafe { size = sizeof(ImDrawVert); }

            declaration = new VertexDeclaration(
                size,

                // Position
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),

                // UV
                new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),

                // Color
                new VertexElement(16, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );
        }
    }
}

