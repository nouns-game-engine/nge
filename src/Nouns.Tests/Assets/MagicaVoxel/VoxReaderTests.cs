using Nouns.Assets.MagicaVoxel;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Nouns.Tests.Assets.MagicaVoxel
{
    public class VoxReaderTests
    {
        private readonly ITestOutputHelper _output;

        public VoxReaderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ReadFiles()
        {
            var paths = Directory.EnumerateFiles("Content", "*.vox", SearchOption.AllDirectories).ToList();
            foreach (var path in paths)
            {
                var vox = VoxReader.ReadFromFile(path);
                _output.WriteLine("File: {0}, Header = {1}, Version = {2}, Chunks = {3}", Path.GetFileName(path),
                    vox.Header?.Trim(), vox.Version, vox.Main?.Children.Count + 1);
            }
        }
    }
}