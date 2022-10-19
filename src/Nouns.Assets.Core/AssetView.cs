using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nouns.Assets.Core
{
    public class AssetView : IAssetProvider, IAssetPathProvider
    {
        private readonly EditorAssetManager owner;

        public string rootDirectory = null!;

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

                case AssetClassification.Embedded:
                case AssetClassification.OutOfPath:
                case AssetClassification.BadExtension:
                default:
                    return null;
            }
        }

        public T? Load<T>(string assetPath) where T : class
        {
            if (rootDirectory == null)
                throw new InvalidOperationException();

            foreach (var extension in AssetReader.Extensions<T>())
            {
                var fullPath = Path.Combine(rootDirectory, assetPath + extension);
                return owner.Load<T>(this, fullPath);
            }

            return null;
        }

        public ICollection<T> LoadAll<T>() where T : class
        {
            return YieldAssetsOfType<T>().ToArray();
        }

        private IEnumerable<T> YieldAssetsOfType<T>() where T : class
        {
            Debug.Assert(rootDirectory != null);

            var pathRoot = Path.GetPathRoot(rootDirectory);
            Debug.Assert(pathRoot != null);

            foreach (var extension in AssetReader.Extensions<T>())
            {
                var starDotExtension = "*" + extension;
                var filePaths = Directory.GetFiles(rootDirectory, starDotExtension, SearchOption.AllDirectories);
                var assetPaths = filePaths.Select(filePath => filePath
                    .Replace(pathRoot, string.Empty)
                    .Replace(Path.GetFileName(filePath), Path.GetFileNameWithoutExtension(filePath)));

                foreach (var assetPath in assetPaths)
                {
                    var asset = Load<T>(assetPath);
                    if (asset != null)
                        yield return asset;
                }
            }
        }
        
        public AssetClassification Classify(object asset, out string? informationalPath)
        {
            var fullPath = owner.GetFullPathFor(asset);
            informationalPath = fullPath;

            if (fullPath == null)
                return AssetClassification.Embedded;

            if (!fullPath.StartsWith(rootDirectory))
                return AssetClassification.OutOfPath;

            foreach (var extension in AssetReader.Extensions(asset.GetType()))
            {
                if (!File.Exists(fullPath))
                    continue;

                if (!fullPath.EndsWith(extension))
                    return AssetClassification.BadExtension;

                var rootLength = rootDirectory.Length + (rootDirectory.EndsWith("\\") ? 0 : 1);
                var assetPath = fullPath.Substring(rootLength, fullPath.Length - rootLength - extension.Length);
                informationalPath = assetPath;
                break;
            }

            return owner.IsMissingAsset(asset) ? AssetClassification.Missing : AssetClassification.Managed;
        }

        public IEnumerable<AssetDetails> GetDetails(IHasReferencedAssets? referencingAsset = null)
        {
            var assets = referencingAsset == null ? owner.GetAllAssets() : referencingAsset.GetReferencedAssets();
            foreach (var asset in assets)
            {
                if (asset == null)
                    continue;

                string? informationalPath;

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