using Microsoft.Xna.Framework;

namespace NGE.Editor
{ 
    public static class ColorExtensions
    {
        public static System.Numerics.Vector4 ToImGuiVector4(this Color color)
        {
            return new System.Numerics.Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
    }
}