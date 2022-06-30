
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Nouns.Assets.MagicaVoxel;

[DebuggerDisplay("RGBA:({Colors?.Count ?? 0} colors)")]
public sealed class Palette : Chunk
{
    public List<Color>? Colors { get; set; }

    internal static Palette Read(string id, ref ReadOnlySpan<byte> span, ref ulong bytesConsumed)
    {
        if(span.Length < 1024)
            throw new InvalidOperationException("invalid RGBA chunk");

        var start = bytesConsumed;
        var palette = new Palette
        {
            Id = id,
            Colors = new List<Color>(256)
        };
        for (var i = 0; i < 256; i++)
        {
            var rgba = span[..4];
            span = span[4..];
            palette.Colors.Add(Color.FromNonPremultiplied(rgba[0], rgba[1], rgba[2], rgba[3]));
            bytesConsumed += 4;
        }

        if (bytesConsumed - start != 1024)
            throw new InvalidOperationException("invalid RGBA chunk");
        
        return palette;
    }
}