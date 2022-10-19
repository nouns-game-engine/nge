using System;

namespace Nouns.Assets.Core;

public interface IAssetReader
{
    string[] Extensions { get; }
    Type Type { get; }
    void Load();
}