using System.IO.Compression;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core;

namespace Nouns.Engine.Pixel2D;

public class AnimationSet
{
    public string? friendlyName;
    public List<Animation> animations = null!;

    public AnimationSet()
    {
        animations = new List<Animation>();
    }

    public AnimationSet(AnimationDeserializeContext context)
    {
        friendlyName = context.br.ReadNullableString();
    }

    public void Serialize(AnimationSerializeContext context)
    {
        context.bw.WriteNullableString(friendlyName);
    }

    public void WriteToFile(string path)
    {
        using var stream = File.Create(path);
        using var zip = new GZipStream(stream, CompressionMode.Compress, true);
        using var bw = new BinaryWriter(zip);

        Serialize(new AnimationSerializeContext(bw));
    }

    public static AnimationSet ReadFromFile(string path, GraphicsDevice graphicsDevice)
    {
        using var stream = File.OpenRead(path);
        using var unzip = new GZipStream(stream, CompressionMode.Decompress, true);
        using var br = new BinaryReader(unzip);

        var context = new AnimationDeserializeContext(br, graphicsDevice);
        return new AnimationSet(context);
    }
}