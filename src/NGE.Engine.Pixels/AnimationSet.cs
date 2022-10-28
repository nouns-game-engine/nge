using System.IO.Compression;
using NGE.Core;
using NGE.Core.Serialization;
using NGE.Engine.Pixels.Serialization;

namespace NGE.Engine.Pixels;

public class AnimationSet : ISerialize<AnimationSerializeContext>, IDeserialize<AnimationDeserializeContext>
{
    public string? friendlyName;
    public List<Animation> animations = null!;

    // ReSharper disable once UnusedMember.Global (Serialization)
    public AnimationSet()
    {
        animations = new List<Animation>();
    }

    public AnimationSet(AnimationDeserializeContext context)
    {
        Deserialize(context);
    }

    public void Deserialize(AnimationDeserializeContext context)
    {
        friendlyName = context.br.ReadNullableString();
    }

    public void Serialize(AnimationSerializeContext context)
    {
        context.bw.WriteNullableString(friendlyName);
    }

    public void WriteToFile(string path, IServiceProvider serviceProvider)
    {
        using var stream = File.Create(path);
        using var zip = new GZipStream(stream, CompressionMode.Compress, true);
        using var bw = new BinaryWriter(zip);

        Serialize(new AnimationSerializeContext(bw, serviceProvider));
    }

    public void ReadFromFile(string path, IServiceProvider serviceProvider)
    {
        using var stream = File.OpenRead(path);
        using var unzip = new GZipStream(stream, CompressionMode.Decompress, true);
        using var br = new BinaryReader(unzip);

        Deserialize(new AnimationDeserializeContext(br, serviceProvider));
    }
}