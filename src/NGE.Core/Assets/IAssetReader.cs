namespace NGE.Core.Assets;

public interface IAssetReader
{
    string[] Extensions { get; }
    Type Type { get; }
    void Load();
}