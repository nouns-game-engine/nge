using Nouns.Assets.Core;
using Nouns.Core;
using Nouns.Engine.Pixel2D.Serialization;
using System.IO.Compression;

namespace Nouns.Engine.Pixel2D;

public class Level : IHasReferencedAssets
{
    public string? friendlyName;
    public string? behaviorName;

    public readonly Dictionary<string, string> properties = new();
    public readonly List<LevelObject> levelObjects;

    public Level()
    {
        levelObjects = new List<LevelObject>();
    }

    public Level(LevelDeserializeContext context)
    {
        friendlyName = context.br.ReadNullableString();
        behaviorName = context.br.ReadNullableString();

        var propertyCount = context.br.ReadInt32();
        for (var i = 0; i < propertyCount; i++)
        {
            properties.Add(context.br.ReadString(), context.br.ReadString());
        }

        int thingsCount = context.br.ReadInt32();
        levelObjects = new List<LevelObject>(thingsCount);
        for (int i = 0; i < thingsCount; i++)
            levelObjects.Add(new LevelObject(context));
    }

    public void WriteToFile(string path, IAssetPathProvider assetPathProvider)
    {
        using var stream = File.Create(path);
        using var zip = new GZipStream(stream, CompressionMode.Compress, true);
        using var bw = new BinaryWriter(zip);

        Serialize(new LevelSerializeContext(bw, assetPathProvider));
    }

    public void Serialize(LevelSerializeContext context)
    {
        context.bw.WriteNullableString(friendlyName);
        context.bw.WriteNullableString(behaviorName);

        context.bw.Write(properties.Count);
        foreach (var kvp in properties)
        {
            context.bw.Write(kvp.Key);
            context.bw.Write(kvp.Value);
        }

        context.bw.Write(levelObjects.Count);
        foreach (var thing in levelObjects)
            thing.Serialize(context);
    }

    #region IHasReferencedAssets Members

    public IEnumerable<object> GetReferencedAssets()
    {
        foreach (var thing in levelObjects)
            yield return thing.AnimationSet;
    }

    public void ReplaceAsset(object search, object replace)
    {
        if (search is not AnimationSet)
            return;

        foreach (var thing in levelObjects)
        {
            if (!ReferenceEquals(thing.AnimationSet, search))
                continue;

            thing.AnimationSet = (AnimationSet)replace;
        }
    }

    #endregion
}