using System;

namespace NGE.Assets;

public interface IAssetReader
{
    string[] Extensions { get; }
    Type Type { get; }
    void Load();
}