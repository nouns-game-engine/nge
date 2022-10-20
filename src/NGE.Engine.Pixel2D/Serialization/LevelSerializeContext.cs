using System.Diagnostics;
using NGE.Assets;
using NGE.Core.Serialization;

namespace NGE.Engine.Pixel2D.Serialization;

public sealed class LevelSerializeContext : ISerializeContext
{
    public const int FormatVersion = 1;

    public int Version { get; }

    public readonly BinaryWriter bw;
    private readonly AnimationSerializeContext animationSerializeContext;
    private readonly IAssetPathProvider assetPathProvider;

    public LevelSerializeContext(BinaryWriter bw, IAssetPathProvider assetPathProvider) : this(bw, assetPathProvider, FormatVersion) { }

    public LevelSerializeContext(BinaryWriter bw, IAssetPathProvider assetPathProvider, int version)
    {
        this.bw = bw;
        this.assetPathProvider = assetPathProvider;

        Version = version;
        if (Version > FormatVersion)
            throw new Exception("Tried to save asset with a version that is too new");

        bw.Write(Version);
        animationSerializeContext = new AnimationSerializeContext(bw);
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