using System;
using System.Collections.Generic;

namespace Nouns.Assets
{
	public class MissingAssetFactory
	{
		public delegate object CreateMissingAsset(IServiceProvider services, string fullPath);

		private static readonly Dictionary<Type, CreateMissingAsset> createRegistry = new();

		public static void Clear()
		{
			createRegistry.Clear();
		}

		public static void Add<T>(CreateMissingAsset createMissingAsset)
		{
			createRegistry.Add(typeof(T), createMissingAsset);
		}

		public static T? Create<T>(IServiceProvider services, string fullPath) where T : class
		{
			if (createRegistry.TryGetValue(typeof(T), out var createMissingAsset))
				return createMissingAsset(services, fullPath) as T;

			throw new InvalidOperationException("Unknown or unsupported asset type");
		}

		public static object Create(Type assetType, IServiceProvider services, string fullPath)
		{
			if (createRegistry.TryGetValue(assetType, out var createMissingAsset))
				return createMissingAsset(services, fullPath);
			throw new InvalidOperationException("Unknown or unsupported asset type");
		}
	}
}