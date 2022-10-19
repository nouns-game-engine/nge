using Nouns.Assets.Core;
using Nouns.Assets.GLTF.Runtime;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Core;
using SharpGLTF.Runtime;
using SharpGLTF.Schema2;

namespace Nouns.Assets.GLTF
{
    public class GltfReader : IAssetReader
    {
        public string Extension => ".gltf";
        public Type Type => typeof(FnaModelInstance);

        public void Load()
        {
            AssetReader.Add<FnaModelInstance>(Extension, (fullPath, _, serviceProvider) => ReadFromFile(fullPath, serviceProvider));
        }

        public static FnaModelInstance ReadFromFile(string path, IServiceProvider serviceProvider)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                throw new FileNotFoundException("could not find GLTF file at specified path", path);

            var model = ModelRoot.Load(path);

            var graphicsDevice = serviceProvider.GetRequiredService<GraphicsDevice>();
            var context = new BasicEffectsLoaderContext(graphicsDevice);
            var meshes = context.CreateRuntimeModels();

            var scene = model.DefaultScene;
            var scenes = new SceneTemplate[1];
            scenes[0] = SceneTemplate.Create(scene);

            var template = new FnaModelTemplate(scenes, 0, meshes);
            var instance = template.CreateInstance(0);
            return instance;
        }
    }
}