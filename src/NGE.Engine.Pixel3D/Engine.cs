using System.Reflection;
using NGE.Engine.Pixel3D.Caching;
using NGE.Engine.StateMachine;

namespace NGE.Engine.Pixel3D
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
