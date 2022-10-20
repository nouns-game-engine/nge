using Microsoft.Xna.Framework.Graphics;

namespace NGE.Assets.GLTF.Runtime
{
    // tracks all the disposable objects of a model;
    // vertex buffers, index buffers, effects and textures.
    class GraphicsResourceTracker
    {
        #region data

        private readonly List<GraphicsResource> _Disposables = new List<GraphicsResource>();        

        #endregion

        #region properties

        public IReadOnlyList<GraphicsResource> Disposables => _Disposables;

        #endregion

        #region API        
        public void AddDisposable(GraphicsResource resource)
        {
            if (resource == null) throw new ArgumentNullException();
            if (_Disposables.Contains(resource)) throw new ArgumentException();
            _Disposables.Add(resource);
        }

        #endregion
    }
}
