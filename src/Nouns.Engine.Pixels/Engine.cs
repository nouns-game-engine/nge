using System.Reflection;
using Nouns.Assets.Core;
using Nouns.Core;
using Nouns.Core.StateMachine;

namespace Nouns.Engine.Pixels
{
    public static class Engine
    {
        public static void RegisterAssets()
        {
            AssetReader.Add<AnimationSet>(".as",
                (fullPath, _, services) => AnimationSet.ReadFromFile(fullPath, services.GraphicsDevice()));
        }

        public static void StartBackgroundLoading(params Assembly[] assemblies)
        {
            StateProvider.Setup(assemblies);
            CreateThingCache.Initialize(assemblies);
        }
    }
}
