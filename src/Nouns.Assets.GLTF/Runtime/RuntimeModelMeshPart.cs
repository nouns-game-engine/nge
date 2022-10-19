using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Assets.GLTF.Runtime
{
    /// <summary>
    /// Replaces <see cref="ModelMeshPart"/>.
    /// </summary>    
    sealed class RuntimeModelMeshPart
    {
        #region lifecycle

        internal RuntimeModelMeshPart(RuntimeModelMesh parent)
        {
            _Parent = parent;
        }

        #endregion

        #region data

        private readonly RuntimeModelMesh _Parent;

        private Effect _Effect;

        private IndexBuffer _IndexBuffer;
        private int _IndexOffset;
        private int _PrimitiveCount;        

        private VertexBuffer _VertexBuffer;
        private int _VertexOffset;
        private int _VertexCount;

        public object Tag { get; set; }

        #endregion

        #region properties

        public Effect Effect
        {
            get => _Effect;
            set
            {
                if (_Effect == value) return;
                _Effect = value;
                _Parent.InvalidateEffectCollection(); // if we change this property, we need to invalidate the parent's effect collection.
            }
        }

        public GraphicsDevice Device => _Parent._GraphicsDevice;

        #endregion

        #region API

        public void SetVertexBuffer(VertexBuffer vb, int offset, int count)
        {
            this._VertexBuffer = vb;
            this._VertexOffset = offset;
            this._VertexCount = count;            
        }

        public void SetIndexBuffer(IndexBuffer ib, int offset, int count)
        {
            this._IndexBuffer = ib;
            this._IndexOffset = offset;
            this._PrimitiveCount = count;            
        }

        public void Draw(GraphicsDevice device)
        {
            if (_PrimitiveCount > 0)
            {
                device.SetVertexBuffer(_VertexBuffer);
                device.Indices = _IndexBuffer;

                for (int j = 0; j < _Effect.CurrentTechnique.Passes.Count; j++)
                {
                    _Effect.CurrentTechnique.Passes[j].Apply();
                    device.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList, _VertexOffset, _IndexOffset, _PrimitiveCount, 0, _PrimitiveCount);
                }
            }
        }

        #endregion
    }
}
