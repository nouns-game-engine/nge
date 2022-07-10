using System;

namespace Nouns.Assets.MagicaVoxel
{
    [Flags]
    public enum Faces
    {
        None = 0,
        Right = 1,
        Left = 2,
        Top = 4,
        Bottom = 8,
        Front = 16,
        Back = 32,
        All = 63
    }
}