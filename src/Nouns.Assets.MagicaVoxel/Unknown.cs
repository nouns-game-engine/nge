using System.Diagnostics;

namespace Nouns.Assets.MagicaVoxel;

[DebuggerDisplay("UNKNOWN: {Id} =: {Children.Count} children")]
public class Unknown : Chunk
{
    public byte[]? Content { get; set; }

    internal static Chunk Read(ref ReadOnlySpan<byte> span, ref ulong bytesConsumed, string? id, int contentBytes)
    {
        var start = bytesConsumed;
        var unknown = new Unknown
        {
            Id = id
        };

        if (contentBytes > 0)
        {
            var content = span[..contentBytes];
            unknown.Content = content.ToArray();
            span = span[contentBytes..];
            bytesConsumed += (ulong)content.Length;
        }

        if (bytesConsumed - start != (ulong) contentBytes)
            throw new InvalidOperationException($"invalid {id} chunk");

        return unknown;
    }
}