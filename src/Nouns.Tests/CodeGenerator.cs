using System.Collections.Generic;
using System.IO.Compression;
using Xunit;
using Xunit.Abstractions;
using SixLabors.ImageSharp.Formats.Png;
using System.IO;
using System.Text.Json;
using Nouns.Graphics.Pipeline;
using SixLabors.ImageSharp;

namespace Nouns.Tests
{
    public class CodeGenerator
    {
        private readonly ITestOutputHelper console;

        public CodeGenerator(ITestOutputHelper console)
        {
            this.console = console;
        }

        [Fact]
        public void ConvertPngsToNounParts()
        {
            const string zipPath = "external/assets_png.zip";

            const string outputDir = "output";
            Directory.CreateDirectory(outputDir);
            
            var decoder = new PngDecoder();
            using var archive = ZipFile.OpenRead(zipPath);

            var rectangles = new Dictionary<string, Rect>();

            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.StartsWith("__MACOSX"))
                    continue; // ignore cached files in canonical archive

                if (!entry.FullName.EndsWith(".png"))
                    continue; // ignore non-png images

                using var stream = entry.Open();

                try
                {
                    const int factor = 16; // archive uses 500, but that's an arbitrary size, when the real scale out factor is 512/32 = 16

                    var (image, rect) = ImageSharpFunctions.CropToEssentialImage(stream, decoder);
                    var imagePath = Path.Combine(outputDir, Path.GetFileName(entry.FullName));
                    image.SaveAsPng(imagePath);
                    ImageMagickFunctions.Resize(imagePath, rect.w / factor, rect.h / factor);

                    var rectangle = new Rect(rect.x / factor, rect.y / factor, rect.w / factor, rect.h / factor);
                    rectangles.Add(Path.GetFileNameWithoutExtension(entry.FullName), rectangle);
                }
                catch (InvalidImageContentException)
                {
                    console.WriteLine(entry.FullName);
                    throw;
                }
            }

            var rectsPath = Path.Combine(outputDir, "rectangles.json");
            File.WriteAllText(rectsPath, JsonSerializer.Serialize(rectangles));

            if(File.Exists("assets_png_sized.zip"))
                File.Delete("assets_png_sized.zip");

            ZipFile.CreateFromDirectory(outputDir, "assets_png_sized.zip");
        }
    }
}
