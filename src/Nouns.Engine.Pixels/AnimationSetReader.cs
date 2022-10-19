using Nouns.Assets.Core;
using Nouns.Core;

namespace Nouns.Engine.Pixels;

// ReSharper disable once UnusedMember.Global
public sealed class AnimationSetReader : IAssetReader
{
    private static readonly string[] extensions = { ".as" };
    public string[] Extensions => extensions;

    public Type Type => typeof(AnimationSet);

    public void Load()
    {
        foreach(var extension in Extensions)
            AssetReader.Add<AnimationSet>(extension, (fullPath, _, services) => AnimationSet.ReadFromFile(fullPath, services.GetGraphicsDevice()));
    }
}