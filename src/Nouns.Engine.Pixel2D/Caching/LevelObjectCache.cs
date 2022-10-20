using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using NGE.Engine;

namespace Nouns.Engine.Pixel2D.Caching;

public static class LevelObjectCache
{
    private delegate Actor CreateLevelObjectDelegate(LevelObject levelObject, UpdateContext context);

    private static readonly Dictionary<string, CreateLevelObjectDelegate> cache = new();

    public static void Initialize(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new[] { typeof(LevelObjectCache).Assembly };

        var constructorTypes = new[] { typeof(LevelObject), typeof(UpdateContext) };

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                TryCreateLevelObjectDelegate(type, constructorTypes);
            }
        }
    }

    private static void TryCreateLevelObjectDelegate(Type type, Type[] constructorTypes)
    {
        if (!typeof(Actor).IsAssignableFrom(type))
            return;

        var constructor = type.GetConstructor(constructorTypes);
        if (constructor != null)
        {
            var dm = new DynamicMethod($"Create_{type.Name}", typeof(Actor), constructorTypes, type);
            var il = dm.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0); // Thing
            il.Emit(OpCodes.Ldarg_1); // UpdateContext
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            cache[type.Name] = (CreateLevelObjectDelegate)dm.CreateDelegate(typeof(CreateLevelObjectDelegate));
        }
        else
        {
            Trace.TraceWarning($"No '{nameof(LevelObject)}' constructor for {type}");
        }
    }

    public static Actor CreateLevelObject(string behaviour, LevelObject levelObject, UpdateContext context)
    {
        return cache[behaviour](levelObject, context);
    }
}