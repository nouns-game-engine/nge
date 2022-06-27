using System.Diagnostics;

namespace Nouns.Assets.MagicaVoxel;

[DebuggerDisplay("XYZI:({X},{Y},{Z},{I})")]
public sealed class Voxel
{
    public byte X { get; set; }
    public byte Y { get; set; }
    public byte Z { get; set; }
    public byte I { get; set; }
}