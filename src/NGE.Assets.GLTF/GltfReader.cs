using NGE.Assets.GLTF.Runtime;
using NGE.Core;

namespace NGE.Assets.GLTF;

// ReSharper disable once UnusedMember.Global
public class GltfReader : IAssetReader
{
    private static readonly string[] extensions = {".gltf", ".glb"};
    public string[] Extensions => extensions;

    public Type Type => typeof(ModelInstance);

    public void Load()
    {
        foreach (var extension in Extensions)
            AssetReader.Add<ModelInstance>(extension, (fullPath, _, serviceProvider) => ReadFromFile(fullPath, serviceProvider));
    }

    public static ModelInstance ReadFromFile(string path, IServiceProvider serviceProvider)
    {
        var fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
            throw new FileNotFoundException("could not find GLB file at specified path", path);

        var graphicsDevice = serviceProvider.GetGraphicsDevice();
        var context = new BasicEffectsLoaderContext(graphicsDevice);

        var content = ModelTemplate.LoadDeviceModel(graphicsDevice, path, context);
        var template = content.Instance;
        var instance = template.CreateInstance(0);

        content.Dispose();
        return instance;
    }
}