using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NGE.Strings;

namespace Nouns.Assets.Core
{
	public static class AssetReader
	{
		public delegate object ReadFromFile(string fullPath, IAssetProvider assetProvider, IServiceProvider serviceProvider);

        private static readonly Dictionary<string, Type> extensionRegistry = new();
		private static readonly Dictionary<string, ReadFromFile> readRegistry = new();

        public static IEnumerable<Type> RegisteredTypes => extensionRegistry.Values;

		public static void Clear()
		{
			extensionRegistry.Clear();
			readRegistry.Clear();
		}

		public static bool CanRead(string extension)
		{
			var extensions = extensionRegistry.Keys;
			foreach(var registered in extensions)
				if (registered == extension)
					return true;
			return false;
		}

		public static void Add<T>(string extension, ReadFromFile readFromFile)
		{
			extensionRegistry.Add(extension, typeof(T));
			readRegistry.Add(extension, readFromFile);
            Trace.TraceInformation($"Added {extension} support for {typeof(T).Name}");
        }
		
		public static Type GetTypeForExtension(string extension)
		{
			foreach (var registered in extensionRegistry)
				if (registered.Key == extension)
					return registered.Value;
            throw new InvalidOperationException(Strings.UnknownAssetExtension);
		}
		
		public static IEnumerable<string> Extensions(Type type)
        {
            EnsureAssetTypeIsRegistered(type);

            foreach (var entry in extensionRegistry)
				if(entry.Value == type)
					yield return entry.Key;
        }

        public static IEnumerable<string> Extensions<T>()
		{
			return Extensions(typeof(T));
		}

		public static T Read<T>(IAssetProvider assetProvider, IServiceProvider services, string fullPath)
			where T : class
		{
            EnsureAssetTypeIsRegistered(typeof(T));

            var extension = Path.GetExtension(fullPath);

            if (!readRegistry.TryGetValue(extension, out var read))
				throw new InvalidOperationException(Strings.UnknownAssetType);

			return (T) read(fullPath, assetProvider, services);
		}

		public static object Read(Type assetType, IAssetProvider assetProvider, IServiceProvider services, string fullPath)
		{
            EnsureAssetTypeIsRegistered(assetType);

            var extension = Path.GetExtension(fullPath);

            if (!readRegistry.TryGetValue(extension, out var read))
				throw new InvalidOperationException(Strings.UnknownAssetType);

			return read(fullPath, assetProvider, services);
		}

        private static void EnsureAssetTypeIsRegistered(Type type)
        {
            Type? registered = null;
            foreach (var entry in extensionRegistry)
            {
                if (entry.Value != type) continue;
                registered = type;
                break;
            }

            if (registered == null)
                throw new InvalidOperationException(Strings.UnknownAssetType);
        }
    }
}