using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Assets.GLTF.Runtime;

/// <summary>
/// Replaces <see cref="ModelMesh"/>
/// </summary>
sealed class RuntimeModelMesh
{
    #region lifecycle

    public RuntimeModelMesh(GraphicsDevice graphicsDevice)
    {
        this._GraphicsDevice = graphicsDevice;
    }

    #endregion

    #region data        

    internal GraphicsDevice _GraphicsDevice;

    private readonly List<RuntimeModelMeshPart> _Primitives = new List<RuntimeModelMeshPart>();

    private IReadOnlyList<Effect> _Effects;

    private Microsoft.Xna.Framework.BoundingSphere? _Sphere;

    #endregion

    #region  properties

    public IReadOnlyCollection<Effect> Effects
    {
        get
        {
            if (_Effects != null) return _Effects;

            // Create the shared effects collection on demand.

            _Effects = _Primitives
                .Select(item => item.Effect)
                .Distinct()
                .ToArray();

            return _Effects;
        }
    }

    public Microsoft.Xna.Framework.BoundingSphere BoundingSphere
    {
        set => _Sphere = value;

        get
        {
            if (_Sphere.HasValue) return _Sphere.Value;

            return default;
        }
            
    }

    public IReadOnlyList<RuntimeModelMeshPart> MeshParts => _Primitives;

    public string Name { get; set; }

    public ModelBone ParentBone { get; set; }

    public object Tag { get; set; }

    #endregion

    #region API

    internal void InvalidateEffectCollection() { _Effects = null; }

    public RuntimeModelMeshPart CreateMeshPart()
    {
        var primitive = new RuntimeModelMeshPart(this);

        _Primitives.Add(primitive);
        InvalidateEffectCollection();

        _Sphere = null;

        return primitive;
    }

    public void Draw()
    {
        for (int i = 0; i < _Primitives.Count; i++)
        {
            _Primitives[i].Draw(_GraphicsDevice);
        }
    }

    #endregion
}