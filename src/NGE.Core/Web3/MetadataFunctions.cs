using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Nethereum.ABI.FunctionEncoding;

namespace NGE.Core.Web3
{
    public static class MetadataFunctions
    {
        public static JsonTokenMetadata? DownloadMetadata(string rpcUrl, string contractAddress, int tokenId)
        {
            try
            {
                var rpc = new Nethereum.Web3.Web3(rpcUrl);
                var service = rpc.Eth.ERC721.GetContractService(contractAddress);
                var tokenUri = service.TokenURIQueryAsync(tokenId).ConfigureAwait(false).GetAwaiter().GetResult();

                if (tokenUri.StartsWith("http://") || tokenUri.StartsWith("https://"))
                {
                    var response = Singleton.http.GetAsync(tokenUri).ConfigureAwait(false).GetAwaiter().GetResult();
                    var buffer = response.Content.ReadAsByteArrayAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var charSet = response.Content.Headers.ContentType?.CharSet ?? "utf-8";
                    var encoding = Encoding.GetEncoding(charSet);
                    tokenUri = encoding.GetString(buffer);
                }

                var json = !DataUri.TryParseApplication(tokenUri, out var format) || format.Data == null
                    ? tokenUri
                    : Encoding.UTF8.GetString(format.Data);

                var metadata = JsonSerializer.Deserialize<JsonTokenMetadata>(json);
                return metadata;
            }
            catch (SmartContractRevertException e)
            {
                Trace.TraceError(e.RevertMessage);
                return null;
            }
        }
    }
}
