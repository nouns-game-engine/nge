using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nouns.Assets
{
    public class AssetView : IAssetProvider, IAssetPathProvider
    {
        private readonly EditorAssetManager owner;

        public string rootDirectory;

        public AssetView(EditorAssetManager owner)
        {
            this.owner = owner;
        }

        public string? GetAssetPath<T>(T asset) where T : class
        {
	        switch (Classify(asset, out var informationalPath))
            {
                case AssetClassification.Managed:
                case AssetClassification.Missing:
                    return informationalPath;

                default:
                    return null;
            }
        }

        public T Load<T>(string assetPath) where T : class
        {
            if (rootDirectory == null)
                throw new InvalidOperationException();

            var fullPath = Path.Combine(rootDirectory, assetPath + AssetReader.Extension<T>());
            return owner.Load<T>(this, fullPath);
        }

        public ICollection<T> LoadAll<T>() where T : class
        {
            return YieldAssetsOfType<T>().ToArray();
        }

        private IEnumerable<T> YieldAssetsOfType<T>() where T : class
        {
            var starDotExtensions = "*" + AssetReader.Extension<T>();
            var filePaths = Directory.GetFiles(rootDirectory, starDotExtensions, SearchOption.AllDirectories);
            var assetPaths = filePaths.Select(filePath =>
                filePath
                    .Replace(Path.GetPathRoot(rootDirectory), string.Empty)
                    .Replace(Path.GetFileName(filePath), Path.GetFileNameWithoutExtension(filePath)));
            foreach (var assetPath in assetPaths)
                yield return Load<T>(assetPath);
        }
        
        public AssetClassification Classify(object asset, out string informationalPath)
        {
            var fullPath = owner.GetFullPathFor(asset);
            informationalPath = fullPath;

            if (fullPath == null)
                return AssetClassification.Embedded;

            if (!fullPath.StartsWith(rootDirectory))
                return AssetClassification.OutOfPath;

            var extension = AssetReader.Extension(asset.GetType());
            if (!fullPath.EndsWith(extension))
                return AssetClassification.BadExtension;

            var rootLength = rootDirectory.Length + (rootDirectory.EndsWith("\\") ? 0 : 1);
            var assetPath = fullPath.Substring(rootLength, fullPath.Length - rootLength - extension.Length);
            informationalPath = assetPath;

            return owner.IsMissingAsset(asset) ? AssetClassification.Missing : AssetClassification.Managed;        }

        public IEnumerable<AssetDetails> GetDetails(IHasReferencedAssets referencingAsset = null)
        {
            var assets = referencingAsset == null ? owner.GetAllAssets() : referencingAsset.GetReferencedAssets();
            foreach (var asset in assets)
            {
                if (asset == null)
                    continue;

                string informationalPath;

                var friendlyName = !(asset is IEditorNameProvider friendlyNameProvider) ? string.Empty : friendlyNameProvider.EditorName;
                var classification = Classify(asset, out informationalPath);

                var details = new AssetDetails
                {
                    Classification = classification,
                    Path = GetAssetPath(asset),
                    FriendlyName = friendlyName,
                    Asset = asset
                };

                yield return details;
            }
        }
    }
}