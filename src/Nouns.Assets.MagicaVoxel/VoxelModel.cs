using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Graphics;

namespace Nouns.Assets.MagicaVoxel
{
    public sealed class VoxelModel
    {
        public List<VertexPositionNormalColor> vertexBuffer = new();
        public List<int> indexBuffer = new();
        public Dictionary<VertexPositionNormalColor, int> vertexMap = new();
        public VertexPositionNormalColor vertex;

        public void Draw(GraphicsDevice graphicsDevice, Effect effect, Vector3 position)
        {
            
        }
    }
}