using System.Diagnostics;
using System.IO;
using Nouns.Editor;

namespace Nouns.Assets
{
	public class AssetDropHandler : IEditorDropHandler
	{
		private readonly EditorAssetManager assetManager;
		private readonly AssetView assetView;

		public bool Enabled => true;

		public AssetDropHandler(EditorAssetManager assetManager, string rootDirectory)
		{
			this.assetManager = assetManager;
            
            assetView = new AssetView(assetManager)
            {
                rootDirectory = rootDirectory
            };
        }
		
		public bool Handle(IEditorContext context, params string[] fullPaths)
		{
			foreach (var fullPath in fullPaths)
			{
				var extension = Path.GetExtension(fullPath);
				if (!AssetReader.CanRead(extension))
				{
					Trace.TraceWarning($"unrecognized asset extension {extension}.");
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
