﻿using System.Diagnostics;
using NGE.Graphics;

namespace NGE.Assets.MagicaVoxel
{
    [DebuggerDisplay("XYZI:({X},{Y},{Z},{I})")]
    public sealed class Voxel
    {
        public byte X { get; set; }
        public byte Y { get; set; }
        public byte Z { get; set; }
        public byte I { get; set; }
        public Faces SharedFaces { get; set; }

        public Point3 AsPoint => new(X, Y, Z);
    }
}