using Microsoft.Xna.Framework;

namespace NGE.Core
{
    public static class BinaryExtensions
    {
        public static bool WriteBoolean(this BinaryWriter bw, bool value)
        {
            bw.Write(value);
            return value;
        }

        public static void WriteNullableString(this BinaryWriter bw, string? value)
        {
            if (bw.WriteBoolean(value != null))
                bw.Write(value!);
        }

        public static string? ReadNullableString(this BinaryReader br)
        {
            return br.ReadBoolean() ? br.ReadString() : null;
        }

        public static void Write(this BinaryWriter bw, Vector2 vector2)
        {
            bw.Write(vector2.X);
            bw.Write(vector2.Y);
        }
    }
}
