using System.Runtime.InteropServices;
using System.Text;

namespace Nouns.Assets.MagicaVoxel;

internal static class SpanExtensions
{
    public static bool TryParse(this ref ReadOnlySpan<byte> source, int length, ref ulong bytesConsumed, out string? value)
    {
        if (source.Length < length)
        {
            value = default;
            return false;
        }

        unsafe
        {
            fixed (byte* buffer = &MemoryMarshal.GetReference(source)) {
                var charCount = Encoding.UTF8.GetCharCount(buffer, length);
                fixed (char* chars = stackalloc char[charCount]) {
                    var count = Encoding.UTF8.GetChars(buffer, length, chars, charCount);
                    value = new string(chars, 0, count);
                    source = source[length..];
                    bytesConsumed += (ulong) length;
                    return true;
                }
            }
        }
    }

    public static bool TryParse(this ref ReadOnlySpan<byte> source, ref ulong bytesConsumed, out int value)
    {
        if (source.Length < 4)
        {
            value = default;
            return false;
        }
        var span = source[..4];
        value = BitConverter.ToInt32(span);
        source = source[4..];
        bytesConsumed += 4;
        return true;
    }
}