using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nouns.Assets.MagicaVoxel
{
    [DebuggerDisplay("CHUNK: {Id}:= {Children.Count} children")]
    public class Chunk
    {
        public string? Id { get; set; }
        public List<Chunk> Children { get; set; } = new();

        internal static Chunk? FromBuffer(ref ReadOnlySpan<byte> span, Chunk? parent, ref ulong bytesConsumed)
        {
            Chunk? chunk;

            if (!span.TryParse(4, ref bytesConsumed, out var id))
                throw new InvalidOperationException("could not read start of chunk");

            if (!span.TryParse(ref bytesConsumed, out var contentBytes))
                throw new InvalidOperationException("could not read contentBytes");

            if (!span.TryParse(ref bytesConsumed, out var childBytes))
                throw new InvalidOperationException("could not read childBytes");

            switch (id)
            {
                case "SIZE":
                    {
                        chunk = Size.Read(id, ref span, ref bytesConsumed);
                        break;
                    }
                case "XYZI":
                    {
                        chunk = Model.Read(id, parent, ref span, ref bytesConsumed);
                        break;
                    }
                case "RGBA":
                    {
                        chunk = Palette.Read(id, ref span, ref bytesConsumed);
                        break;
                    }
                case "PACK":
                    {
                        chunk = Pack.Read(ref span, ref bytesConsumed, id);
                        break;
                    }
                case "MAIN":
                    {
                        chunk = new Chunk
                        {
                            Id = id
                        };
                        break;
                    }
                default:
                    chunk = Unknown.Read(ref span, ref bytesConsumed, id, contentBytes);
                    break;
            }

            while (childBytes > 0)
            {
                var start = bytesConsumed;
                var child = FromBuffer(ref span, chunk, ref bytesConsumed);
                if (child != null)
                    chunk?.Children.Add(child);

                var bytesRead = bytesConsumed - start;
                childBytes -= (int)bytesRead;
            }

            return chunk;
        }
    }
}