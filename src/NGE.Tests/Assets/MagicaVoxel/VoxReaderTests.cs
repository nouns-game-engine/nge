using System.IO;
using System.Linq;
using NGE.Assets.MagicaVoxel;
using Xunit;
using Xunit.Abstractions;

namespace NGE.Tests.Assets.MagicaVoxel
{
    public class VoxReaderTests
    {
        private readonly ITestOutputHelper console;

        public VoxReaderTests(ITestOutputHelper console)
        {
            this.console = console;
        }

        [Fact]
        public void ReadFiles()
        {
            var paths = Directory.EnumerateFiles("Content", "*.vox", SearchOption.AllDirectories).ToList();
            foreach (var path in paths)
            {
                var vox = VoxReader.ReadFromFile(path);
                console.WriteLine("File: {0}, Header = {1}, Version = {2}, Chunks = {3}", Path.GetFileName(path),
                    vox.Header?.Trim(), vox.Version, vox.Main?.Children.Count + 1);
            }
        }
    }
}