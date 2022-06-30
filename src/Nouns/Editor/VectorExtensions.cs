using Microsoft.Xna.Framework;

namespace Nouns.Editor;

public static class VectorExtensions
{
    public static System.Numerics.Vector3 ToImGuiVector3(this Vector3 vector)
    {
        return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
    }

    public static Vector3 ToVector3(this System.Numerics.Vector3 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }
}