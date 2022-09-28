using Microsoft.Xna.Framework;
using Nouns.Editor;

namespace Nouns.Engine.Pixels
{
    public struct Position
    {
        public int X;
        public int Y;
        public int Z;

        public Position(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(Point xy, int z = 0)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

        [NonEditable]
        public Vector2 ToDisplay => new(X, -(Y + Z));

        [NonEditable]
        public Vector2 ToDisplayNoTransform => new(X, Y);
    }
}
