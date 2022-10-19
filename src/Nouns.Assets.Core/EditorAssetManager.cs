using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nouns.Assets.Core
{
	public class EditorAssetManager
	{
		public EditorAssetManager(IServiceProvider services)
		{
			Services = services;
		}

		public IServiceProvider Services { get; }
		
		#region Asset Replacement

		private void ReplaceAssetReferences(object search, object replace)
		{
			foreach (var cachedAsset in assetToPathLookup.Keys)
			{
                if (cachedAsset is IHasReferencedAssets hasReferencedAssets)
					hasReferencedAssets.ReplaceAsset(search, replace);
			}
		}

		#endregion
		
		#region Path

		public static string GuessAssetDirectoryFrom(string fullPath)
		{
			var assetsFolderName = $"{Path.DirectorySeparatorChar}assets{Path.DirectorySeparatorChar}";

			fullPath = Path.GetFullPath(fullPath);
			var index = fullPath.IndexOf(assetsFolderName, StringComparison.InvariantCultureIgnoreCase);
			if (index == -1)
				return null;
			return fullPath.Substring(0, index + assetsFolderName.Length);
		}

		#endregion

        #region Private Asset Lookups

		private readonly HashSet<object> missingAssets = new(ReferenceEqualityComparer.Instance);
        private Dictionary<string, object> pathToAssetLookup = new();
		private readonly Dictionary<object, string> assetToPathLookup = new(ReferenceEqualityComparer.Instance);

		private void AddAsset(string fullPath, object asset, bool isMissing = false)
		{
			pathToAssetLookup.Add(fullPath, asset);
			assetToPathLookup.Add(asset, fullPath);

			if (isMissing)
				missingAssets.Add(asset);
		}

		private void RemoveAsset(object asset)
		{
            assetToPathLookup.TryGetValue(asset, out var path);
			if (path != null)
				pathToAssetLookup.Remove(path);

			assetToPathLookup.Remove(asset);
			missingAssets.Remove(asset);
		}

		private void RemoveByPath(string fullPath)
		{
			var asset = pathToAssetLookup[fullPath];
			assetToPathLookup.Remove(asset);
			pathToAssetLookup.Remove(fullPath);
			missingAssets.Remove(asset);
		}

		public bool IsMissingAsset(object asset)
		{
			return missingAssets.Contains(asset);
		}

		public IEnumerable<object> GetAllAssets()
		{
			return assetToPathLookup.Keys;
		}

		#endregion
		
		#region Asset Loading

		public static bool ReadOrCreateMissing<T>(AssetView assetView, IServiceProvider services, string fullPath,
			out T asset) where T : class
		{
			if (!File.Exists(fullPath))
			{
				asset = MissingAssetFactory.Create<T>(services, fullPath);
				return false;
			}

			try
			{
				asset = AssetReader.Read<T>(assetView, services, fullPath);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error while loading asset of type " + typeof(T).Name + " from \"" + fullPath + "\"");
				Debug.WriteLine(e);
				Debug.WriteLine("");

				asset = MissingAssetFactory.Create<T>(services, fullPath);
				return false;
			}
		}

		public static bool ReadOrCreateMissing(Type assetType, AssetView assetView, IServiceProvider services, string fullPath,
			out object? asset)
		{
			if (!File.Exists(fullPath))
			{
				asset = MissingAssetFactory.Create(assetType, services, fullPath);
				return false;
			}

			try
			{
				asset = AssetReader.Read(assetType, assetView, services, fullPath);
				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine($"Error while loading asset of type {assetType.Name} from \"{fullPath}\"");
				Debug.WriteLine(e);
				Debug.WriteLine("");

                try
                {
                    asset = MissingAssetFactory.Create(assetType, services, fullPath);
                    return false;
                }
                catch (Exception me)
                {
                    Debug.WriteLine($"Error while loading missing asset from factory for type {assetType.Name}");
                    Debug.WriteLine(me);
                    Debug.WriteLine("");

                    asset = null;
                    return false;
                }
			}
		}
		
		public static string NormalizePath(string path)
		{
			if (path.Contains('/'))
				path = path.Replace('/', '\\');
			return path;
		}

		public T Load<T>(AssetView assetView, string fullPath) where T : class
		{
			fullPath = NormalizePath(fullPath);
			if (pathToAssetLookup.TryGetValue(fullPath, out var asset))
				return (T) asset;
			var missing = !ReadOrCreateMissing(assetView, Services, fullPath, out T typedAsset);
			AddAsset(fullPath, typedAsset, missing);
			return typedAsset;
		}

		public object Load(Type assetType, AssetView assetView, string fullPath)
		{
			fullPath = NormalizePath(fullPath);
			if (pathToAssetLookup.TryGetValue(fullPath, out var asset))
				return asset;
			var missing = !ReadOrCreateMissing(assetType, assetView, Services, fullPath, out asset);
			AddAsset(fullPath, asset, missing);
			return asset;
		}

		public string? GetFullPathFor(object asset)
		{
			assetToPathLookup.TryGetValue(asset, out var path);
			return path;
		}

		#endregion
		
		#region User Loaded Assets

		public void UserStartTracking<T>(T asset) where T : class
		{
			if (!assetToPathLookup.ContainsKey(asset))
				assetToPathLookup.Add(asset, null!);
		}

		public void UserStartTracking(object asset)
		{
			if (!assetToPathLookup.ContainsKey(asset))
				assetToPathLookup.Add(asset, null!);
		}
		
		public T? UserLoadFromCache<T>(string fullPath) where T : class
		{
			fullPath = NormalizePath(fullPath);

			if (!pathToAssetLookup.TryGetValue(fullPath, out var asset))
				return null;

			if (IsMissingAsset(asset))
				return null;

			if (asset is T typed)
				return typed;

			return null;
		}

		public object? UserLoadFromCache(string fullPath)
		{
			fullPath = NormalizePath(fullPath);
			return pathToAssetLookup.TryGetValue(fullPath, out var asset) ? IsMissingAsset(asset) ? null : asset : null;
		}
		
		public T UserLoadAndRevert<T>(AssetView assetView, string fullPath) where T : class
		{
			var newAssetWasFound = ReadOrCreateMissing(assetView, Services, fullPath, out T newAsset);
			Debug.Assert(newAssetWasFound);

			if (pathToAssetLookup.TryGetValue(fullPath, out var oldAsset))
			{
				ReplaceAssetReferences(oldAsset, newAsset);
				RemoveAsset(oldAsset);
			}

			AddAsset(fullPath, newAsset, !newAssetWasFound);

			return newAsset;
		}
		
		public object UserLoadAndRevert(Type assetType, AssetView assetView, string fullPath)
		{
			var newAssetWasFound = ReadOrCreateMissing(assetType, assetView, Services, fullPath, out var newAsset);
			Debug.Assert(newAssetWasFound);

			if (pathToAssetLookup.TryGetValue(fullPath, out var oldAsset))
			{
				ReplaceAssetReferences(oldAsset, newAsset);
				RemoveAsset(oldAsset);
			}

			AddAsset(fullPath, newAsset, !newAssetWasFound);

			return newAsset;
		}

        public void UserSaveRename<T>(T asset, string newFullPath) where T : class
		{
			assetToPathLookup.TryGetValue(asset, out var oldFullPath);
			if (oldFullPath != null)
				pathToAssetLookup.Remove(oldFullPath);

			if (newFullPath != null)
				pathToAssetLookup[newFullPath] = asset;
			assetToPathLookup[asset] = newFullPath;

			missingAssets.Remove(asset);
		}

		public void UserSaveRename(object asset, string newFullPath)
		{
			assetToPathLookup.TryGetValue(asset, out var oldFullPath);
			if (oldFullPath != null)
				pathToAssetLookup.Remove(oldFullPath);

			if (newFullPath != null)
				pathToAssetLookup[newFullPath] = asset;
			assetToPathLookup[asset] = newFullPath;

			missingAssets.Remove(asset);
		}

		public void UserCloseMaybeRevert<T>(AssetView assetView, T asset) where T : class
		{
			var inUse = assetToPathLookup.Keys
				.Select(a => a as IHasReferencedAssets)
				.Where(a => a != null)
				.SelectMany(a => a.GetReferencedAssets())
				.Any(a => ReferenceEquals(a, asset));

			if (inUse)
			{
				assetToPathLookup.TryGetValue(asset, out var fullPath);
				if (fullPath != null)
					UserLoadAndRevert<T>(assetView, fullPath);
			}
			else
			{
				RemoveAsset(asset);
			}
		}

		public void UserCloseMaybeRevert(Type assetType, AssetView assetView, object asset)
		{
			var inUse = assetToPathLookup.Keys
				.Select(a => a as IHasReferencedAssets)
				.Where(a => a != null)
				.SelectMany(a => a.GetReferencedAssets())
				.Any(a => ReferenceEquals(a, asset));

			if (inUse)
			{
				assetToPathLookup.TryGetValue(asset, out var fullPath);
				if (fullPath != null)
					UserLoadAndRevert(assetType, assetView, fullPath);
			}
			else
			{
				RemoveAsset(asset);
			}
		}

		public void UserCompactCache(IEnumerable<object> allUserKnownAssets)
		{
			var assetsToKeep = new HashSet<object>(ReferenceEqualityComparer.Instance);
			foreach (var asset in allUserKnownAssets)
				UserCompactCacheRecursiveHelper(asset, assetsToKeep);

			var assetsToRemove = new HashSet<object>(ReferenceEqualityComparer.Instance);
			foreach (var asset in assetToPathLookup.Keys)
				if (!assetsToKeep.Contains(asset))
					assetsToRemove.Add(asset);
			
			foreach (var asset in assetsToRemove)
				assetToPathLookup.Remove(asset);
			missingAssets.ExceptWith(assetsToRemove);

			pathToAssetLookup = assetToPathLookup.Where(kvp => kvp.Value != null)
				.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
			
			GC.Collect();
		}

		private static void UserCompactCacheRecursiveHelper(object asset, ISet<object> assetsToKeep)
		{
			if (assetsToKeep.Add(asset))
			{
				if (!(asset is IHasReferencedAssets referencing))
					return;

				foreach (var reference in referencing.GetReferencedAssets())
					UserCompactCacheRecursiveHelper(reference, assetsToKeep);
			}
		}

		#endregion
	}
}