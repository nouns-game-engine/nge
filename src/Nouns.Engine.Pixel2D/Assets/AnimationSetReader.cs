﻿using NGE.Assets;

namespace Nouns.Engine.Pixel2D.Assets;

// ReSharper disable once UnusedMember.Global (Reflection)
public sealed class AnimationSetReader : IAssetReader
{
    private static readonly string[] extensions = { ".as" };
    public string[] Extensions => extensions;

    public Type Type => typeof(AnimationSet);

    public void Load()
    {
        foreach (var extension in Extensions)
            AssetReader.Add<AnimationSet>(extension, (fullPath, _, serviceProvider) => AnimationSet.ReadFromFile(fullPath, serviceProvider));
    }
}