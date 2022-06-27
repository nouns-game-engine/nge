using System.Diagnostics;

namespace Nouns.Assets.MagicaVoxel;

[DebuggerDisplay("SIZE:({X},{Y},{Z})")]
public sealed class Size : Chunk
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Z { get; set; }

    internal static Chunk Read(string id, ref ReadOnlySpan<byte> span, ref ulong bytesConsumed)
    {
        var start = bytesConsumed;

        var size = new Size
        {
            Id = id
        };

        if (span.TryParse(ref bytesConsumed, out var x))
            size.X = x;
        if (span.TryParse(ref bytesConsumed, out var y))
            size.Y = y;
        if (span.TryParse(ref bytesConsumed, out var z))
            size.Z = z;

        if (bytesConsumed - start != 12)
            throw new InvalidOperationException("invalid SIZE chunk");

        return size;
    }
}