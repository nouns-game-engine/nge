using System;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Nouns.Editor;
using Microsoft.Xna.Framework;

namespace Nouns.Snaps
{
    internal sealed class Web3Menu : IEditorMenu
    {
        private readonly IConfiguration configuration;

        public Web3Menu(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool Enabled => true;

        public string Label => "Web3";

        public string? RpcUrl
        {
            get => Web3()["rpcUrl"];
            set => Web3()["rpcUrl"] = value;
        }

        private IConfigurationSection Web3() => configuration.GetSection("Web3");

        public void Layout(IEditingContext context, GameTime gameTime)
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

            if(!valid)
                ImGui.PopStyleColor();
        }
    }
}
