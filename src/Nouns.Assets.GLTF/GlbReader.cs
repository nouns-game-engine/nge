using Microsoft.Xna.Framework.Graphics;
using Nouns.Assets.Core;
using Nouns.Assets.GLTF.Runtime;
using Nouns.Core;
using SharpGLTF.Runtime;
using SharpGLTF.Schema2;

namespace Nouns.Assets.GLTF;

public class GlbReader : IAssetReader
{
    public string Extension => ".glb";
    public Type Type => typeof(ModelInstance);

    public void Load()
    {
        AssetReader.Add<ModelInstance>(Extension, (fullPath, _, serviceProvider) => ReadFromFile(fullPath, serviceProvider));
    }

    public static ModelInstance ReadFromFile(string path, IServiceProvider serviceProvider)
    {
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
            throw new FileNotFoundException("could not find GLTF file at specified path", path);

        var model = ModelRoot.Load(path);

        var graphicsDevice = serviceProvider.GetGraphicsDevice();
        var context = new BasicEffectsLoaderContext(graphicsDevice);
        
        var deviceContent = ModelTemplate.CreateDeviceModel(graphicsDevice, model, context);
        var template = deviceContent.Instance;
        var instance = template.CreateInstance(0);
        return instance;
    }
}