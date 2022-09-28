using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core.Web3;

namespace Nouns.Graphics.Pipeline
{
    public static class Web3Functions
    {
        public static (Texture2D?, Vector2?) GetTextureAndSizeFromToken(GraphicsDevice graphicsDevice, Uri rpcUri, string contractAddress, int tokenId) => GetTextureAndSizeFromToken(graphicsDevice, rpcUri.ToString(), contractAddress, tokenId);

        public static (Texture2D?, Vector2?) GetTextureAndSizeFromToken(GraphicsDevice graphicsDevice, string rpcUrl, string contractAddress, int tokenId)
        {
            var metadata = MetadataFunctions.DownloadMetadata(rpcUrl, contractAddress, tokenId);
            if (metadata == null || string.IsNullOrWhiteSpace(metadata.Image))
                return (null, null);

            if (!DataUri.TryParseImage(metadata.Image, out var format) || format.Data == null)
            {
                if (!metadata.Image.StartsWith("http://") && !metadata.Image.StartsWith("https://"))
                    return (null, null);

                var response = Singleton.http.GetAsync(metadata.Image).ConfigureAwait(false).GetAwaiter().GetResult();
                var buffer = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                var urlExtension = Path.GetExtension(metadata.Image);
                if(!string.IsNullOrWhiteSpace(urlExtension))
                    format.Extension = urlExtension[1..];

                format.Data = buffer;
            }

            Texture2D? texture;

            switch (format.Extension)
            {
                case "gif":
                case "png":
                case "bmp":
                {
                    var stream = new MemoryStream(format.Data);
                    texture = Texture2D.FromStream(graphicsDevice, stream);
                    break;
                }
                case "svg":
                {
                    var svg = Encoding.UTF8.GetString(format.Data);
                    var stream = SvgFunctions.SvgToPng(svg);
                    texture = Texture2D.FromStream(graphicsDevice, stream);
                    break;
                }
                default:
                    throw new NotSupportedException($"No support for {format.Extension} images yet.");
            }

            return (texture, new Vector2(texture.Width, texture.Height));
        }
    }
}
