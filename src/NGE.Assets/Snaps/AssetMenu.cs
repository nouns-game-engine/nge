using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using NGE.Core;
using NGE.Editor;

namespace NGE.Assets.Snaps
{
    // ReSharper disable once UnusedMember.Global
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

        private bool queueUpdate = true;
        private readonly Dictionary<Type, string[]> registrations = new();

        public void UpdateLayout(IEditingContext context, GameTime gameTime)
        {
            if (!queueUpdate)
                return;

            registrations.Clear();
            foreach (var registration in AssetReader.RegisteredTypes)
            {
                if (registrations.TryGetValue(registration, out _)) continue;
                registrations.Add(registration, AssetReader.Extensions(registration).ToArray());
            }

            queueUpdate = false;
        }

        public void DrawLayout(IEditingContext context, GameTime gameTime)
        {
            var assetDirectory = AssetDirectory;
            
            if (ImGui.InputText("Asset Directory", ref assetDirectory, 1000))
                AssetDirectory = assetDirectory;

            if (ImGui.BeginMenu("Readers"))
            {
                foreach (var registration in registrations.OrderBy(x => x.Key.Name))
                {
                    foreach (var extension in registration.Value)
                    {
                        ImGui.Text($"{registration.Key.Name} ({extension})");
                    }
                }

                ImGui.EndMenu();
            }
        }
    }
}
