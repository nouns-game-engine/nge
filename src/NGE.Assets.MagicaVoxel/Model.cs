using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NGE.Assets.MagicaVoxel
{
    [DebuggerDisplay("Model: {Size}=: {Voxels.Count} voxels")]
    public sealed class Model : Chunk
    {
        public Size? Size { get; set; }
        public List<Voxel>? Voxels { get; set; }

        internal static Chunk Read(string id, Chunk? parent, ref ReadOnlySpan<byte> span, ref ulong bytesConsumed)
        {
            var start = bytesConsumed;
            var size = parent?.Children.LastOrDefault() as Size;
            var model = new Model
            {
                Id = id,
                Size = size
            };
            if (span.TryParse(ref bytesConsumed, out var voxelCount))
            {
                model.Voxels = new List<Voxel>(voxelCount);
                for (var i = 0; i < voxelCount; i++)
                {
                    var xyzi = span[..4];
                    span = span[4..];
                    var voxel = new Voxel
                    {
                        X = xyzi[0],
                        Y = xyzi[1],
                        Z = xyzi[2],
                        I = xyzi[3]
                    };
                    bytesConsumed += 4;
                    model.Voxels.Add(voxel);
                }
            }

            if (bytesConsumed - start != (ulong)(4 + voxelCount * 4))
                throw new InvalidOperationException("invalid XYZI chunk");

            return model;
        }
    }
}