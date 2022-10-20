using Nouns.Assets.Core;
using Nouns.Engine.Pixel2D.Serialization;
using System.IO.Compression;
using NGE.Core;
using NGE.Core.Serialization;

namespace Nouns.Engine.Pixel2D;

public class Level : IHasReferencedAssets, ISerialize<LevelSerializeContext>, IDeserialize<LevelDeserializeContext>
{
    public string? friendlyName;
    public string? behaviorName;

    public readonly Dictionary<string, string> properties = new();
    public List<LevelObject> levelObjects = new();

    // ReSharper disable once UnusedMember.Global (Serialization)
    public Level() { }

    public Level(LevelDeserializeContext context)
    {
        Deserialize(context);
    }

    public void Deserialize(LevelDeserializeContext context)
    {
        friendlyName = context.br.ReadNullableString();
        behaviorName = context.br.ReadNullableString();

        var propertyCount = context.br.ReadInt32();
        for (var i = 0; i < propertyCount; i++)
        {
            properties.Add(context.br.ReadString(), context.br.ReadString());
        }

        var thingsCount = context.br.ReadInt32();
        levelObjects = new List<LevelObject>(thingsCount);
        for (var i = 0; i < thingsCount; i++)
            levelObjects.Add(new LevelObject(context));
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

    public void WriteToFile(string path, IServiceProvider serviceProvider)
    {
        using var stream = File.Create(path);
        using var zip = new GZipStream(stream, CompressionMode.Compress, true);
        using var bw = new BinaryWriter(zip);

        Serialize(new LevelSerializeContext(bw, serviceProvider.GetRequiredService<IAssetPathProvider>()));
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