using Microsoft.Xna.Framework.Graphics;

namespace Nouns.Assets.GLTF.Runtime;

class TextureFactory
{
    #region lifecycle

    public TextureFactory(GraphicsDevice device, GraphicsResourceTracker disposables)
    {
        _Device = device;
        _Disposables = disposables;
    }

    #endregion

    #region data

    private readonly GraphicsDevice _Device;
    private readonly GraphicsResourceTracker _Disposables;

    private readonly Dictionary<SharpGLTF.Memory.MemoryImage, Texture2D> _Textures = new Dictionary<SharpGLTF.Memory.MemoryImage, Texture2D>();        

    #endregion

    #region API

    public Texture2D UseTexture(SharpGLTF.Memory.MemoryImage image, string name = null)
    {
        if (_Device == null) throw new InvalidOperationException();

        if (!image.IsValid) return null;

        if (_Textures.TryGetValue(image, out Texture2D tex)) return tex;

        using (var m = image.Open())
        {
            tex = Texture2D.FromStream(_Device, m);
            _Disposables.AddDisposable(tex);

            tex.Name = name;

            _Textures[image] = tex;

            return tex;
        }
    }

    public Texture2D UseWhiteImage()
    {
        const string solidWhitePNg = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAACXBIWXMAAA7DAAAOwwHHb6hkAAAAFHpUWHRUaXRsZQAACJkrz8gsSQUABoACIippo0oAAAAoelRYdEF1dGhvcgAACJkLy0xOzStJVQhIzUtMSS1WcCzKTc1Lzy8BAG89CQyAoFAQAAAAGklEQVQoz2P8//8/AymAiYFEMKphVMPQ0QAAVW0DHZ8uFaIAAAAASUVORK5CYII=";

        var toBytes = Convert.FromBase64String(solidWhitePNg);

        return UseTexture(new ArraySegment<byte>(toBytes), "_InternalSolidWhite");
    }

    #endregion        
}