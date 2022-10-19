using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Nouns.Assets.Core
{
	public static class AssetReader
	{
		public delegate object ReadFromFile(string fullPath, IAssetProvider assetProvider, IServiceProvider serviceProvider);

        private static readonly Dictionary<Type, string> extensionRegistry = new();
		private static readonly Dictionary<Type, ReadFromFile> readRegistry = new();

        public static IEnumerable<Type> RegisteredTypes => extensionRegistry.Keys;

		public static void Clear()
		{
			extensionRegistry.Clear();
			readRegistry.Clear();
		}

		public static bool CanRead(string extension)
		{
			var extensions = extensionRegistry.Values;
			foreach(var registered in extensions)
				if (registered == extension)
					return true;
			return false;
		}

		public static void Add<T>(string extension, ReadFromFile read)
		{
			extensionRegistry.Add(typeof(T), extension);
			readRegistry.Add(typeof(T), read);
            Trace.TraceInformation($"Added {extension} support for {typeof(T).Name}");
        }
		
		public static Type GetTypeForExtension(string extension)
		{
			foreach (var registered in extensionRegistry)
				if (registered.Value == extension)
					return registered.Key;
            throw new InvalidOperationException("Unknown asset extension");
		}
		
		public static string Extension(Type type)
		{
			if (!extensionRegistry.TryGetValue(type, out var extension))
				throw new InvalidOperationException("Unknown asset type");
			return extension;
		}
		
		public static string Extension<T>()
		{
			return Extension(typeof(T));
		}

		public static T Read<T>(IAssetProvider assetProvider, IServiceProvider services, string fullPath)
			where T : class
		{
			if (!readRegistry.TryGetValue(typeof(T), out var read))
				throw new InvalidOperationException("Unknown asset type");

			return (T) read(fullPath, assetProvider, services);
		}

		public static object Read(Type assetType, IAssetProvider assetProvider, IServiceProvider services, string fullPath)
		{
			if (!readRegistry.TryGetValue(assetType, out var read))
				throw new InvalidOperationException("Unknown asset type");

			return read(fullPath, assetProvider, services);
		}
	}
}