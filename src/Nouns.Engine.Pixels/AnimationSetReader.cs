using Nouns.Assets.Core;
using Nouns.Core;

namespace Nouns.Engine.Pixels;

// ReSharper disable once UnusedMember.Global
public sealed class AnimationSetReader : IAssetReader
{
    public string Extension => ".as";
    public Type Type => typeof(AnimationSet);

    public void Load()
    {
        AssetReader.Add<AnimationSet>(".as",
            (fullPath, _, services) => AnimationSet.ReadFromFile(fullPath, services.GetGraphicsDevice()));
    }
}