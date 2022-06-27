namespace Nouns.Assets.MagicaVoxel;

public sealed class Pack : Chunk
{
    public int ModelCount { get; set; }

    public static Chunk Read(ref ReadOnlySpan<byte> span, ref ulong bytesConsumed, string id)
    {
        var start = bytesConsumed;
        var pack = new Pack
        {
            Id = id
        };

        if (span.TryParse(ref bytesConsumed, out var modelCount))
            pack.ModelCount = modelCount;

        if (bytesConsumed - start != 4)
            throw new InvalidOperationException("invalid PACK chunk");

        return pack;
    }
}