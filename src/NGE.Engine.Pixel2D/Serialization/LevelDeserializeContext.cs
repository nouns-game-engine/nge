using NGE.Assets;
using NGE.Core;
using NGE.Core.Serialization;

namespace NGE.Engine.Pixel2D.Serialization;

public sealed class LevelDeserializeContext : IDeserializeContext
{
    public int Version { get; }

    public readonly BinaryReader br;
    private readonly AnimationDeserializeContext animationDeserializeContext;
    private readonly IAssetProvider assetProvider;

    public LevelDeserializeContext(BinaryReader br, IServiceProvider serviceProvider)
    {
        this.br = br;
        this.assetProvider = serviceProvider.GetRequiredService<IAssetProvider>();

        Version = br.ReadInt32();

        if (Version > LevelSerializeContext.FormatVersion)
            throw new Exception("Tried to load asset with a version that is too new");
        
        animationDeserializeContext = new AnimationDeserializeContext(br, serviceProvider);
    }

    public AnimationSet ReadAnimationSet()
    {
        var isReference = br.ReadBoolean();
        if (isReference)
            return assetProvider.Load<AnimationSet>(br.ReadString())!;

        return new AnimationSet(animationDeserializeContext);
    }
}