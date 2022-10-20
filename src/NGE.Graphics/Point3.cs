using System;
using Microsoft.Xna.Framework;

namespace NGE.Graphics
{
    public readonly struct Point3 : IEquatable<Point3>
    {
        public readonly int x, y, z;

        public Point3(int x, int y, int z)
        {
            this.x = (byte)x;
            this.y = (byte)y;
            this.z = (byte)z;
        }

        public Point3 Add(Point3 other) => new(x + other.x, y + other.y, z + other.z);

        public Vector3 ToVector3() => new(x, y, z);

        public bool Equals(Point3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override bool Equals(object obj)
        {
            return obj is Point3 other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static bool operator ==(Point3 left, Point3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point3 left, Point3 right)
        {
            return !left.Equals(right);
        }
    }
}