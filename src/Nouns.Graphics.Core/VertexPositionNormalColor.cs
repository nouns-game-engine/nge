using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Graphics.Core
{
    public struct VertexPositionNormalColor : IVertexType, IEquatable<VertexPositionNormalColor>
    {
        public Vector3 position;
        public Vector3 normal;
        public Color color;

        private static readonly VertexDeclaration vertexDeclaration = new(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0));

        public VertexDeclaration VertexDeclaration => vertexDeclaration;

        public VertexPositionNormalColor(Vector3 position, Vector3 normal, Color color)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
        }

        public bool Equals(VertexPositionNormalColor other)
        {
            return position.Equals(other.position) && normal.Equals(other.normal) && color.Equals(other.color);
        }

        public override bool Equals(object? obj)
        {
            return obj is VertexPositionNormalColor other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position, normal, color);
        }

        public static bool operator ==(VertexPositionNormalColor left, VertexPositionNormalColor right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(VertexPositionNormalColor left, VertexPositionNormalColor right)
        {
            return !left.Equals(right);
        }
    }
}
