using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NGE.Core;
using Nouns.Editor;
using Nouns.Graphics.Pipeline;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = System.Numerics.Vector2;

namespace NGE.Snaps
{
    // ReSharper disable once UnusedMember.Global
    public sealed class Web3Menu : IEditorMenu
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly ImGuiRenderer imGui;
        private readonly IConfiguration configuration;

        public Web3Menu(IServiceProvider serviceProvider)
        {
            graphicsDevice = serviceProvider.GetGraphicsDevice();
            imGui = serviceProvider.GetRequiredService<ImGuiRenderer>();
            configuration = serviceProvider.GetRequiredService<IConfiguration>();
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
        private Vector2? lastImportedSize;

        private int index = 0;

        public void DrawLayout(IEditingContext context, GameTime gameTime)
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
            
            var values = configuration.GetSection("web3.knownContracts");
            if (values != null)
            {
                var list = values.AsEnumerable()
                    .Select(x => x.Key.Replace("web3.knownContracts", "").Replace(":", ""))
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToArray();

                if (ImGui.Combo("Known Contracts", ref index, list, list.Length))
                    ContractAddress = values[list[index]];
            }
            
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

                if (tokenId >= 0 && !string.IsNullOrWhiteSpace(contractAddress) && ImGui.Button("Import"))
                {
                    var (texture, size) =
                        Web3Functions.GetTextureAndSizeFromToken(graphicsDevice, rpcUrl, contractAddress, tokenId);
                    if (texture != null)
                        lastImportedTexture = imGui.BindTexture(texture);
                    if (size.HasValue)
                        lastImportedSize = new Vector2(size.Value.X, size.Value.Y);
                }
            }

            if (lastImportedTexture.HasValue)
            {
                var size = lastImportedSize.HasValue ? new Vector2(lastImportedSize.Value.X, lastImportedSize.Value.Y) : new Vector2(320, 320);
                ImGui.Image(lastImportedTexture.Value, size);
            }
        }
    }
}
