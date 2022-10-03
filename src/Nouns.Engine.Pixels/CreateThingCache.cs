using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Nouns.Engine.Pixels;

public static class CreateThingCache
{
    private delegate Actor CreateThingDelegate(Thing thing, UpdateContext context);

    private static readonly Dictionary<string, CreateThingDelegate> cache = new();

    public static void Initialize(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new[] { typeof(CreateThingCache).Assembly };

        var constructorTypes = new[] { typeof(Thing), typeof(UpdateContext) };

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                TryCreateThingDelegate(type, constructorTypes);
            }
        }
    }

    private static void TryCreateThingDelegate(Type type, Type[] constructorTypes)
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

            cache[type.Name] = (CreateThingDelegate)dm.CreateDelegate(typeof(CreateThingDelegate));
        }
        else
        {
            Trace.TraceWarning($"No 'Thing' constructor for {type}");
        }
    }

    public static Actor CreateThing(string behaviour, Thing thing, UpdateContext context)
    {
        return cache[behaviour](thing, context);
    }
}