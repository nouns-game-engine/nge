using NGE.Assets;
using NGE.Engine.Pixel3D.Serialization;

namespace NGE.Engine.Pixel3D.Assets;

// ReSharper disable once UnusedMember.Global (Reflection)
public sealed class LevelReader : DeserializeAssetReader<Level, LevelDeserializeContext>
{
    public LevelReader() : base(".lvl") { }
}