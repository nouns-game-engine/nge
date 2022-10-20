using ImageMagick;

namespace NGE.Pipeline;

public static class ImageMagickFunctions
{
    public static void Resize(string imagePath, int width, int height)
    {
        var image = new MagickImage(imagePath);
        image.FilterType = FilterType.Point;
        image.Interpolate = PixelInterpolateMethod.Nearest;
        image.Resize(width, height);
        image.Format = MagickFormat.Png32;

        image.Write(imagePath, MagickFormat.Png32);
    }
}