using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace Nouns.Assets.MagicaVoxel
{
        public static class VoxReader
    {
        public static VoxFile FromFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new FileNotFoundException($"Could not find VOX file at specified path", path);

            using var file = MemoryMappedFile.CreateFromFile(path, FileMode.Open);
            using var view = file.CreateViewStream();
            using var handle = view.SafeMemoryMappedViewHandle;
            unsafe
            {
                try
                {
                    var ptr = (byte*)0;
                    handle.AcquirePointer(ref ptr);
                    var length = Math.Min((int)fileInfo.Length, (int)handle.ByteLength);
                    var span = new ReadOnlySpan<byte>(ptr, length);
                    return FromBuffer(span);
                }
                finally
                {
                    handle.ReleasePointer();
                }
            }
        }

        public static VoxFile FromBuffer(ReadOnlySpan<byte> span)
        {
            var length = (ulong)span.Length;

            var bytesConsumed = 0UL;

            var vox = new VoxFile();

            if (span.TryParse(4, ref bytesConsumed, out var header))
                vox.Header = header;

            if (span.TryParse(ref bytesConsumed, out var version))
                vox.Version = version;

            vox.Main = Chunk.FromBuffer(ref span, null, ref bytesConsumed);

            if (span.Length != 0 || length != bytesConsumed)
                throw new InvalidOperationException("did not parse file fully");

            return vox;
        }
    }
}