using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Text.Json;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Nouns.Editor;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nethereum.Web3;
using Nouns.Core.Web3;
using Nouns.Pipeline;
using Vector2 = System.Numerics.Vector2;

namespace Nouns.Snaps
{
    internal sealed class Web3Menu : IEditorMenu
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly ImGuiRenderer imGui;
        private readonly IConfiguration configuration;

        public Web3Menu(GraphicsDevice graphicsDevice, ImGuiRenderer imGui, IConfiguration configuration)
        {
            this.graphicsDevice = graphicsDevice;
            this.imGui = imGui;
            this.configuration = configuration;
        }

        public bool Enabled => true;

        public string Label => "Web3";

        public string? RpcUrl
        {
            get => Web3()["rpcUrl"];
            set => Web3()["rpcUrl"] = value;
        }

        public string? ContractAddress { get; set; }

        public int TokenId { get; set; }
        
        private IConfigurationSection Web3() => configuration.GetSection("Web3");

        private IntPtr? lastImportedTexture;

        public void Layout(IEditingContext context, GameTime gameTime)
        {
            RpcUrlTextBox();

            if (!string.IsNullOrWhiteSpace(RpcUrl) && Uri.TryCreate(RpcUrl, UriKind.Absolute, out var rpcUrl))
            {
                var contractAddress = !string.IsNullOrWhiteSpace(ContractAddress)
                    ? ContractAddress
                    : "0x9c8ff314c9bc7f6e59a9d9225fb22946427edc03";

                if (ImGui.InputText("Contract Address", ref contractAddress, 42))
                    ContractAddress = contractAddress ?? "0x9c8ff314c9bc7f6e59a9d9225fb22946427edc03";
                
                var tokenId = TokenId;
                if (ImGui.InputInt("Token ID", ref tokenId, 1))
                    TokenId = Math.Max(0, tokenId);

                var canMakeCall = tokenId >= 0 && !string.IsNullOrWhiteSpace(contractAddress);

                if (canMakeCall && ImGui.Button("Import"))
                {
                    var rpc = new Web3(rpcUrl.ToString());
                    var service = rpc.Eth.ERC721.GetContractService(contractAddress);
                    var tokenUri = service.TokenURIQueryAsync(tokenId).ConfigureAwait(false).GetAwaiter().GetResult();

                    var encoded = tokenUri["data:application/json;base64,".Length..];
                    var payload = Convert.FromBase64String(encoded);
                    var json = Encoding.UTF8.GetString(payload);
                    var metadata = JsonSerializer.Deserialize<JsonTokenMetadata>(json);

                    if (metadata != null && !string.IsNullOrWhiteSpace(metadata.Image))
                    {
                        encoded = metadata.Image["data:image/svg+xml;base64,".Length..];
                        payload = Convert.FromBase64String(encoded);

                        var svg = Encoding.UTF8.GetString(payload);
                        var stream = SvgFunctions.SvgToPng(svg);
                        var texture = Texture2D.FromStream(graphicsDevice, stream);
                        lastImportedTexture = imGui.BindTexture(texture);
                    }
                }
            }

            if (lastImportedTexture.HasValue)
            {
                ImGui.Image(lastImportedTexture.Value, new Vector2(320, 320));
            }
        }

        private void RpcUrlTextBox()
        {
            var url = RpcUrl ?? "http://localhost:8545";

            var valid = true;

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                url = uri.ToString();
            }
            else
            {
                valid = false;
                ImGui.PushStyleColor(ImGuiCol.FrameBg, Color.DarkRed.PackedValue);
            }

            if (ImGui.InputText("RPC URL", ref url, 1024))
            {
                RpcUrl = url;
            }

            if (!valid)
                ImGui.PopStyleColor();
        }
    }
}
