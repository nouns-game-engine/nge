using Svg;
using System.Drawing.Imaging;
using System.Text;

namespace Nouns.Pipeline
{
    public static class SvgFunctions
    {
        public static MemoryStream SvgToPng(string svgFileContents)
        {
            var buffer = Encoding.UTF8.GetBytes(svgFileContents);
            using var ms = new MemoryStream(buffer);
            var document = SvgDocument.Open<SvgDocument>(ms);
            var bitmap = document.Draw();

            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            return stream;
        }
    }
}
