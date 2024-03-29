﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NGE.Assets
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

		public string? GetAssetPath<T>(T asset) where T : class
		{
			loadedAssetPaths.TryGetValue(asset, out var assetPath);
			return assetPath;
		}

		#endregion

		#region Asset Root Directory

		private string? rootDirectory;

		public string? RootDirectory
		{
		    get => rootDirectory;
		    set
			{
				if (loadedAssets.Count == 0)
					rootDirectory = NormalizeAssetPath(value);
				else
					throw new InvalidOperationException(Strings.Strings.CannotChangeAssetRootDirectoryAfterLoadingAssets);
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

		public T? Load<T>(string assetPath) where T : class
		{
            var normalized = NormalizeAssetPath(assetPath);

            assetPath = normalized ?? throw new InvalidOperationException(Strings.Strings.UninitializedAssetPath);

            if (loadedAssets.TryGetValue(assetPath, out var asset))
				return (T) asset;

			if (Locked)
				throw new InvalidOperationException(Strings.Strings.AssetManagerIsLocked);

            Debug.Assert(rootDirectory != null);

            foreach (var extension in AssetReader.Extensions<T>())
            {
                
                var fullPath = Path.Combine(rootDirectory, assetPath + extension);
                Debug.Assert(!fullPath.Contains("\\\\"));
                if (!File.Exists(fullPath))
                    continue;

                var typedAsset = AssetReader.Read<T>(this, Services, fullPath);
                loadedAssets.Add(assetPath, typedAsset);
                loadedAssetPaths.Add(typedAsset, assetPath);
                return typedAsset;
            }

            return null;
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

            foreach (var extension in AssetReader.Extensions<T>())
            {
                var starDotExtension = "*" + extension;
                var filePaths = Directory.GetFiles(rootDirectory, starDotExtension, SearchOption.AllDirectories);

                Debug.Assert(rootDirectory != null);
                var assetPaths = filePaths.Select(filePath => filePath.Replace(rootDirectory, "").Replace(Path.GetFileName(filePath), Path.GetFileNameWithoutExtension(filePath)));

                foreach (var assetPath in assetPaths)
                {
                    var asset = Load<T>(assetPath);
                    if (asset != null)
                        yield return asset;
                }
            }
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