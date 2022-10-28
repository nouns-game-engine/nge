using System.Diagnostics;
using NGE.Assets;
using NGE.Core;
using NGE.Core.Serialization;
using NGE.Engine.Pixels;
using NGE.Engine.Pixels.Serialization;

namespace NGE.Engine.Pixel3D.Serialization;

public sealed class LevelSerializeContext : ISerializeContext
{
    public const int FormatVersion = 1;

    public int Version { get; }

    public readonly BinaryWriter bw;
    private readonly AnimationSerializeContext animationSerializeContext;
    private readonly IAssetPathProvider assetPathProvider;

    public LevelSerializeContext(BinaryWriter bw, IServiceProvider serviceProvider) : this(bw, serviceProvider, FormatVersion) { }

    public LevelSerializeContext(BinaryWriter bw, IServiceProvider serviceProvider, int version)
    {
        this.bw = bw;
        this.assetPathProvider = serviceProvider.GetRequiredService<IAssetPathProvider>();

        Version = version;
        if (Version > FormatVersion)
            throw new Exception("Tried to save asset with a version that is too new");

        bw.Write(Version);
        animationSerializeContext = new AnimationSerializeContext(bw, serviceProvider);
    }

    public void WriteAnimationSet(AnimationSet animationSet)
    {
        var name = assetPathProvider.GetAssetPath(animationSet);
        Debug.Assert(name == null || !name.StartsWith("\\"));

        if (name != null)
        {
            bw.Write(true);
            bw.Write(name);
        }
        else
        {
            bw.Write(false);
            animationSet.Serialize(animationSerializeContext);
        }
    }
}