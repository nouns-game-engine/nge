using System;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Nouns.Core;
using Nouns.Editor;

namespace Nouns.Assets.Core.Snaps
{
    public sealed class AssetMenu : IEditorMenu
    {
        private readonly IConfiguration configuration;
        
        public bool Enabled => true;
        public string Label => "Assets";

        public string AssetDirectory
        {
            get => GetLocations()["assetDirectory"];
            set => GetLocations()["assetDirectory"] = value;
        }

        private IConfigurationSection GetLocations()
        {
            return configuration.GetSection("locations");
        }

        public AssetMenu(IServiceProvider serviceProvider)
        {
            configuration = serviceProvider.GetRequiredService<IConfiguration>();
        }

        public void Layout(IEditingContext context, GameTime gameTime)
        {
            var assetDirectory = AssetDirectory;
            if (ImGui.InputText("Asset Directory", ref assetDirectory, 1000))
                AssetDirectory = assetDirectory;
        }
    }
}
