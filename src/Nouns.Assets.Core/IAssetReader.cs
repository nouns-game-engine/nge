using System;

namespace Nouns.Assets.Core;

public interface IAssetReader
{
    string Extension { get; }
    Type Type { get; }
    void Load();
}