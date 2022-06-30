using Microsoft.Xna.Framework;

namespace Nouns.Editor;

public static class ColorExtensions
{
    public static System.Numerics.Vector4 ToImGuiVector4(this Color color)
    {
        return new System.Numerics.Vector4(color.A, color.B, color.G, color.R);
    }
}