using Nethereum.ABI.FunctionEncoding;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Nouns.Core.Web3
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

                var encoded = tokenUri["data:application/json;base64,".Length..];
                var payload = Convert.FromBase64String(encoded);
                var json = Encoding.UTF8.GetString(payload);
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
