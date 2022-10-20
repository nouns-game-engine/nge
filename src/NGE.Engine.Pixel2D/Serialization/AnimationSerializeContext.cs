using NGE.Core.Serialization;

namespace NGE.Engine.Pixel2D.Serialization;

public class AnimationSerializeContext : ISerializeContext
{
    public AnimationSerializeContext(BinaryWriter bw) : this(bw, FormatVersion) { }

    public AnimationSerializeContext(BinaryWriter bw, int version)
    {
        Version = version;
        this.bw = bw;
        if (Version > FormatVersion)
            throw new Exception("Tried to save asset with a version that is too new");
        bw.Write(Version);
    }

    public readonly BinaryWriter bw;

    #region Version

    public const int FormatVersion = 1;

    public int Version { get; }

    #endregion
}