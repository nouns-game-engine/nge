using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using Nouns.Assets.Core;

namespace Nouns.Assets.MagicaVoxel
{
    public static class VoxReader
    {
        public static void Initialize()
        {
            AssetReader.Add<VoxelModel>(".vox", (fullPath, _, _) => ReadFromFile(fullPath).ToModel());
        }

        public static VoxFile ReadFromFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("could not find VOX file at specified path", path);

#if !WASM
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
                    return ReadFromBuffer(span);
                }
                finally
                {
                    handle.ReleasePointer();
                }
            }
#else
            return ReadFromBuffer(File.ReadAllBytes(path));
#endif
        }
        
        public static VoxFile ReadFromBuffer(ReadOnlySpan<byte> span)
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