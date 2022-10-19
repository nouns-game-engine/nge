using System.Reflection;
using Nouns.Engine.Core.StateMachine;

namespace Nouns.Engine.Pixel2D
{
    public static class Engine
    {
        public static void Initialize(params Assembly[] assemblies)
        {
            StateProvider.Setup(assemblies);
            CreateThingCache.Initialize(assemblies);
            LevelBehaviorCache.Initialize(assemblies);
        }
    }
}
