using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Nouns.Pipeline
{
    public static class ImageSharpFunctions
    {
        public static (Image<Rgba32> cropped, Rect rect) CropToEssentialImage(Stream stream, IImageDecoder decoder)
        {
            var original = Image.Load<Rgba32>(stream, decoder);

            var min = new Point(int.MaxValue, int.MaxValue);
            var max = new Point(int.MinValue, int.MinValue);

            for (var x = 0; x < original.Width; ++x)
            {
                for (var y = 0; y < original.Height; ++y)
                {
                    var pixel = original[x, y];
                    if (pixel.A != 0)
                    {
                        if (x < min.X) min.X = x;
                        if (y < min.Y) min.Y = y;

                        if (x > max.X) max.X = x;
                        if (y > max.Y) max.Y = y;
                    }
                }
            }

            var rectangle = new Rectangle(min.X, min.Y, max.X - min.X + 1, max.Y - min.Y + 1);
            
            // deal with fully transparent images
            if (rectangle.X == int.MaxValue)
                rectangle.X = 0;
            if (rectangle.Y == int.MaxValue)
                rectangle.Y = 0;
            if (rectangle.Width <= 0)
                rectangle.Width = original.Width;
            if (rectangle.Height <= 0)
                rectangle.Height = original.Height;

            var cropped = original.Clone();
            cropped.Mutate(x => x.Crop(new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height)));

            return (cropped, new Rect { x = rectangle.X, y = rectangle.Y, w = rectangle.Width, h = rectangle.Height });
        }
    }
}
