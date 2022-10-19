using Microsoft.Xna.Framework.Graphics;
using Nouns.Assets.Core;

namespace Nouns.Engine.Pixel2D.Serialization;

public sealed class LevelDeserializeContext
{
    public int Version { get; }

    public readonly BinaryReader br;
    private readonly AnimationDeserializeContext animationDeserializeContext;
    private readonly IAssetProvider assetProvider;

    public LevelDeserializeContext(BinaryReader br, IAssetProvider assetProvider, GraphicsDevice device)
    {
        this.br = br;
        this.assetProvider = assetProvider;

        Version = br.ReadInt32();

        if (Version > LevelSerializeContext.FormatVersion)
            throw new Exception("Tried to load asset with a version that is too new");
        
        animationDeserializeContext = new AnimationDeserializeContext(br, device);
    }

    public AnimationSet ReadAnimationSet()
    {
        var isReference = br.ReadBoolean();
        if (isReference)
            return assetProvider.Load<AnimationSet>(br.ReadString())!;

        return new AnimationSet(animationDeserializeContext);
    }
}