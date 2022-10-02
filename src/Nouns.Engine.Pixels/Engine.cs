using Nouns.Assets.Core;
using Nouns.Core;

namespace Nouns.Engine.Pixels
{
    public static class Engine
    {
        public static void RegisterAssets()
        {
            AssetReader.Add<AnimationSet>(".as",
                (fullPath, _, services) => AnimationSet.ReadFromFile(fullPath, services.GraphicsDevice()));
        }
    }
}
