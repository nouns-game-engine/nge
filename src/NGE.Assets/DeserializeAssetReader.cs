using System;
using NGE.Core.Assets;
using NGE.Core.Serialization;

namespace NGE.Assets;

public abstract class DeserializeAssetReader<TAsset, TContext> : IAssetReader 
    where TAsset : IDeserialize<TContext> 
    where TContext : IDeserializeContext
{
    public string[] Extensions { get; }
    public Type Type => typeof(TAsset);

    protected DeserializeAssetReader(params string[] extensions) => Extensions = extensions;

    public virtual void Load()
    {
        foreach (var extension in Extensions)
            AssetReader.Add<TAsset>(extension, (fullPath, _, serviceProvider) =>
            {
                var asset = Activator.CreateInstance<TAsset>();
                asset.ReadFromFile(fullPath, serviceProvider);
                return asset;
            });
    }
}