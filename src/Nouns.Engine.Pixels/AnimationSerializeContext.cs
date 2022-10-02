namespace Nouns.Engine.Pixels;

public class AnimationSerializeContext
{
    public AnimationSerializeContext(BinaryWriter bw) : this(bw, FormatVersion) { }

    public AnimationSerializeContext(BinaryWriter bw, int version)
    {
        this.Version = version;
        this.bw = bw;
        if (Version > FormatVersion)
            throw new Exception("tried to save asset with a version that is too new");
        bw.Write(Version);
    }

    public readonly BinaryWriter bw;

    #region Version

    public const int FormatVersion = 1;

    public int Version { get; }

    #endregion
}