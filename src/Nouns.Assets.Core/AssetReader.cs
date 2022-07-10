using System;
using System.Collections.Generic;

namespace Nouns.Assets.Core
{
	public static class AssetReader
	{
		public delegate object ReadFromFile(string fullPath, IAssetProvider assetProvider, object serviceObject);

		public delegate object ServiceObjectProvider(IServiceProvider services);

		private static readonly Dictionary<Type, string> ExtensionRegistry = new();
		private static readonly Dictionary<Type, ReadFromFile> ReadRegistry = new();

		public static ServiceObjectProvider serviceObjectProvider;

		public static void Clear()
		{
			ExtensionRegistry.Clear();
			ReadRegistry.Clear();
		}

		public static bool CanRead(string extension)
		{
			var extensions = ExtensionRegistry.Values;
			foreach(var registered in extensions)
				if (registered == extension)
					return true;
			return false;
		}

		public static void Add<T>(string extension, ReadFromFile read)
		{
			ExtensionRegistry.Add(typeof(T), extension);
			ReadRegistry.Add(typeof(T), read);
		}
		
		public static Type Type(string extension)
		{
			foreach (var registered in ExtensionRegistry)
				if (registered.Value == extension)
					return registered.Key;
			return default;
		}
		
		public static string Extension(Type type)
		{
			if (!ExtensionRegistry.TryGetValue(type, out var extension))
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
			if (!ReadRegistry.TryGetValue(typeof(T), out var read))
				throw new InvalidOperationException("Unknown asset type");
			var serviceObject = serviceObjectProvider(services);
			return (T) read(fullPath, assetProvider, serviceObject);
		}

		public static object Read(Type assetType, IAssetProvider assetProvider, IServiceProvider services, string fullPath)
		{
			if (!ReadRegistry.TryGetValue(assetType, out var read))
				throw new InvalidOperationException("Unknown asset type");
			var serviceObject = serviceObjectProvider(services);
			return read(fullPath, assetProvider, serviceObject);
		}
	}
}