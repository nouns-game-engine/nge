using System;
using System.Linq;
using ImGuiNET;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using NGE.Core;
using Nouns.Editor;

namespace Nouns.Assets.Core.Snaps;

// ReSharper disable once UnusedMember.Global
public sealed class AssetListWindow : IEditorWindow
{
    private readonly EditorAssetManager editorAssetManager;
    private readonly AssetView assetView;

    public bool Enabled => editorAssetManager.GetAllAssets().Any();
    public ImGuiWindowFlags Flags => ImGuiWindowFlags.AlwaysAutoResize;
    public string? Label => "Assets";
    public string Shortcut => null!;
    public int Width => 0;
    public int Height => 0;

    public AssetListWindow(IServiceProvider serviceProvider)
    {
        editorAssetManager = serviceProvider.GetRequiredService<EditorAssetManager>();
        assetView = new AssetView(editorAssetManager);
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        assetView.rootDirectory = configuration.GetSection("locations")["assetDirectory"];
    }

    public void DrawLayout(IEditingContext context, GameTime gameTime, ref bool opened)
    {
        var assets = editorAssetManager.GetAllAssets().ToList();

        if (ImGui.BeginTable("Asset View", 2))
        {
            ImGui.TableSetupColumn("Informational Path");
            ImGui.TableSetupColumn("Classification");
            ImGui.TableHeadersRow();

            for (var row = 0; row < assets.Count; row++)
            {
                var asset = assets[row];
                var classification = assetView.Classify(asset, out var informationalPath);

                ImGui.TableNextRow();
                ImGui.TableSetColumnIndex(0);
                ImGui.Text(!string.IsNullOrWhiteSpace(informationalPath) ? informationalPath : "<None>");

                ImGui.TableSetColumnIndex(1);
                ImGui.Text(classification.ToString());

            }

            ImGui.EndTable();
        }
    }
}