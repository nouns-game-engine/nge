using NGE.Assets;

namespace NGE.Engine.Pixel2D.Assets;

// ReSharper disable once UnusedMember.Global (Reflection)
public sealed class LevelReader : IAssetReader
{
    private static readonly string[] extensions = { ".lvl" };
    public string[] Extensions => extensions;

    public Type Type => typeof(Level);

    public void Load()
    {
        foreach (var extension in Extensions)
            AssetReader.Add<Level>(extension, (fullPath, _, services) =>
            {
                var level = new Level();
                level.ReadFromFile(fullPath, services);
                return level;
            });
    }
}