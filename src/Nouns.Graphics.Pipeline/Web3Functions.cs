using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core.Web3;

namespace Nouns.Graphics.Pipeline
{
    public static class Web3Functions
    {
        public static Texture2D? GetTextureFromToken(GraphicsDevice graphicsDevice, Uri rpcUri, string contractAddress, int tokenId) => GetTextureFromToken(graphicsDevice, rpcUri.ToString(), contractAddress, tokenId);

        public static Texture2D? GetTextureFromToken(GraphicsDevice graphicsDevice, string rpcUrl, string contractAddress, int tokenId)
        {
            var metadata = MetadataFunctions.DownloadMetadata(rpcUrl, contractAddress, tokenId);
            if (metadata == null || string.IsNullOrWhiteSpace(metadata.Image))
                return null;

            var encoded = metadata.Image["data:image/svg+xml;base64,".Length..];
            var payload = Convert.FromBase64String(encoded);

            var svg = Encoding.UTF8.GetString(payload);
            var stream = SvgFunctions.SvgToPng(svg);
            var texture = Texture2D.FromStream(graphicsDevice, stream);
            return texture;
        }
    }
}
