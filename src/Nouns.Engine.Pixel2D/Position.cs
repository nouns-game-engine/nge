using Microsoft.Xna.Framework;
using NGE.Editor;

namespace Nouns.Engine.Pixel2D
{
    public struct Position
    {
        public int x;
        public int y;
        public int z;

        public Position(int x, int y, int z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Position(Point xy, int z = 0)
        {
            x = xy.X;
            y = xy.Y;
            this.z = z;
        }

        [NonEditable]
        public Vector2 ToDisplay => new(x, -(y + z));

        [NonEditable]
        public Vector2 ToDisplayNoTransform => new(x, y);
    }
}
