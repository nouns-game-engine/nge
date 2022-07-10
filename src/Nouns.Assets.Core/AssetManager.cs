using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Nouns.Assets
{
	public class AssetManager : IAssetProvider, IAssetPathProvider
	{
		public AssetManager(IServiceProvider services, string rootDirectory)
		{
			Services = services;
			RootDirectory = rootDirectory;
		}

		public IServiceProvider Services { get; }

		#region IAssetPathProvider

		public string GetAssetPath<T>(T asset) where T : class
		{
			loadedAssetPaths.TryGetValue(asset, out var assetPath);
			return assetPath;
		}

		#endregion

		#region Asset Root Directory

		private string rootDirectory;

		public string? RootDirectory
		{
		    get => rootDirectory;
		    set
			{
				if (loadedAssets.Count == 0)
					rootDirectory = NormalizeAssetPath(value);
				else
					throw new InvalidOperationException("Cannot change the asset root directory after loading assets");
			}
		}

		#endregion

		#region Managed Assets

		public static string? NormalizeAssetPath(string? assetPath)
		{
			if (assetPath == null)
				return null;
			if (assetPath.Contains('/'))
				assetPath = assetPath.Replace('/', '\\');
			if (assetPath.EndsWith("\\"))
				assetPath = assetPath.Substring(0, assetPath.Length - 1);
			if (assetPath.StartsWith("\\"))
				assetPath = assetPath.Substring(1, assetPath.Length - 1);
			return assetPath;
		}

		private readonly Dictionary<string, object> loadedAssets = new();
        private readonly Dictionary<object, string> loadedAssetPaths = new();

		public void Insert<T>(string assetPath, T asset) where T : class
		{
			loadedAssets.Add(assetPath, asset);
			loadedAssetPaths.Add(asset, assetPath);
		}

		public T Load<T>(string assetPath) where T : class
		{
			assetPath = NormalizeAssetPath(assetPath);

			if (loadedAssets.TryGetValue(assetPath, out var asset))
				return (T) asset;

			if (Locked)
				throw new InvalidOperationException("Asset manager has been locked, cannot load from disk.");

			var fullPath = Path.Combine(rootDirectory, assetPath + AssetReader.Extension<T>());

			Debug.Assert(!fullPath.Contains("\\\\"));
			var typedAsset = AssetReader.Read<T>(this, Services, fullPath);
			loadedAssets.Add(assetPath, typedAsset);
			loadedAssetPaths.Add(typedAsset, assetPath);
			return typedAsset;
		}

		public object Load(Type assetType, string assetPath)
		{
			assetPath = NormalizeAssetPath(assetPath);

			if (loadedAssets.TryGetValue(assetPath, out var asset))
				return asset;

			if (Locked)
				throw new InvalidOperationException("Asset manager has been locked, cannot load from disk.");

			var fullPath = Path.Combine(rootDirectory, assetPath + AssetReader.Extension(assetType));

			Debug.Assert(!fullPath.Contains("\\\\"));
			var typedAsset = AssetReader.Read(assetType, this, Services, fullPath);
			loadedAssets.Add(assetPath, typedAsset);
			loadedAssetPaths.Add(typedAsset, assetPath);
			return typedAsset;
		}

		public ICollection<T> LoadAll<T>() where T : class
		{
			return YieldAssetsOfType<T>().ToArray();
		}

		private IEnumerable<T> YieldAssetsOfType<T>() where T : class
		{
			if (rootDirectory == null)
			{
				Debug.Assert(Locked && loadedAssets.Count > 0);
				foreach (var asset in loadedAssets)
				{
					if (asset.Value is T typed)
						yield return typed;
				}

				yield break;
			}

			var starDotExtension = "*" + AssetReader.Extension<T>();
			var filePaths = Directory.GetFiles(rootDirectory, starDotExtension, SearchOption.AllDirectories);
			var assetPaths = filePaths.Select(filePath => filePath.Replace(RootDirectory, "").Replace(Path.GetFileName(filePath), Path.GetFileNameWithoutExtension(filePath)));

			foreach (var assetPath in assetPaths)
				yield return Load<T>(assetPath);
		}

		#endregion

		#region Locking

		public bool Locked { get; private set; }

		public void Lock()
		{
			Locked = true;
		}

		#endregion
	}
}