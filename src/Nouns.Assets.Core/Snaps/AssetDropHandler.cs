using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Nouns.Core;
using Nouns.Editor;

namespace Nouns.Assets.Snaps
{
    // ReSharper disable once UnusedMember.Global
    public class AssetDropHandler : IEditorDropHandler
    {
        private readonly EditorAssetManager assetManager;
        private readonly AssetView assetView;

        public bool Enabled => true;

        public AssetDropHandler(IServiceProvider services)
        {
            assetManager = services.GetRequiredService<EditorAssetManager>();
            assetView = new AssetView(assetManager);

            var configuration = services.GetRequiredService<IConfiguration>();
            var rootDirectory = configuration.GetSection("locations")["assetDirectory"];
            assetView.rootDirectory = rootDirectory;
        }

        public bool Handle(IEditingContext context, params string[] fullPaths)
        {
            foreach (var fullPath in fullPaths)
            {
                var extension = Path.GetExtension(fullPath);
                if (!AssetReader.CanRead(extension))
                {
                    Trace.TraceWarning($"unrecognized asset extension {extension}");
                    continue;
                }

                var asset = assetManager.UserLoadFromCache(fullPath);
                if (asset == null)
                {
                    var assetType = AssetReader.GetTypeForExtension(extension);
                    asset = assetManager.UserLoadAndRevert(assetType, assetView, fullPath);
                }

                assetManager.UserStartTracking(asset);
            }

            return true;
        }
    }
}
