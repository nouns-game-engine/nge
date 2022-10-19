using System.Reflection;
using Nouns.Engine.Core.StateMachine;
using Nouns.Engine.Pixel2D.Caching;

namespace Nouns.Engine.Pixel2D
{
    public static class Engine
    {
        public static void Initialize(params Assembly[] assemblies)
        {
            StateProvider.Setup(assemblies);
            LevelObjectCache.Initialize(assemblies);
            LevelBehaviorCache.Initialize(assemblies);
        }
    }
}
