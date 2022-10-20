using Microsoft.Xna.Framework.Graphics;
using SharpGLTF.Schema2;

namespace NGE.Assets.GLTF.Runtime;

class EffectsFactory
{
    #region lifecycle

    public EffectsFactory(GraphicsDevice device, GraphicsResourceTracker disposables)
    {
        _Device = device;
        _TexFactory = new TextureFactory(device, disposables);
        _Disposables = disposables;
    }

    #endregion

    #region data

    private readonly GraphicsDevice _Device;
    private readonly TextureFactory _TexFactory;
    private readonly GraphicsResourceTracker _Disposables;

    private readonly Dictionary<Object, Effect> _RigidEffects = new Dictionary<Object, Effect>();
    private readonly Dictionary<Object, SkinnedEffect> _SkinnedEffects = new Dictionary<Object, SkinnedEffect>();
        
    #endregion

    #region API - Schema

    public void Register(Object key, bool isSkinned, Effect effect)
    {
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (effect == null) throw new ArgumentNullException(nameof(effect));

        if (isSkinned && effect is SkinnedEffect skEffect) { _SkinnedEffects[key] = skEffect; }
        else { _RigidEffects[key] = effect; }
    }        

    public Effect GetMaterial(SharpGLTF.Schema2.Material srcMaterial, bool isSkinned)
    {
        if (isSkinned)
        {
            if (_SkinnedEffects.TryGetValue(srcMaterial, out SkinnedEffect dstMaterial)) return dstMaterial;
        }
        else
        {
            if (_RigidEffects.TryGetValue(srcMaterial, out Effect dstMaterial)) return dstMaterial;
        }

        return null;
    }        

    internal Texture2D UseTexture(MaterialChannel? channel, string name)
    {
        if (!channel.HasValue) return _TexFactory.UseWhiteImage();

        if (channel.HasValue && name == null)
        {
            name = channel.Value.LogicalParent.Name;
            if (name == null) name = "null";
            name += $"-{channel.Value.Key}";
        }            

        return _TexFactory.UseTexture(channel.Value.Texture?.PrimaryImage?.Content ?? default, name);
    }

    #endregion
}