using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using NGE.Engine;
using NGE.Engine.StateMachine;

namespace Nouns.Engine.Pixel2D.Caching;

public class LevelBehaviorCache
{
    private delegate LevelBehavior CreateLevelBehaviourDelegate(Level level, UpdateContext context);
    private delegate ILevelBehavior CreateLevelSubBehaviorDelegate(Level level, UpdateContext context);

    private static readonly Dictionary<string, CreateLevelBehaviourDelegate> levelCache = new();
    private static readonly Dictionary<string, CreateLevelSubBehaviorDelegate> levelSubCache = new();
    private static readonly Dictionary<string, CreateLevelSubBehaviorDelegate> globalSubCache = new();

    public static IEnumerable<string> LevelBehaviors => levelCache.Keys;

    private static readonly ReadOnlyList<ILevelBehavior> noSubBehaviors = new(new List<ILevelBehavior>(0));

    public static void Initialize(params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
            assemblies = new[] { typeof(LevelBehaviorCache).Assembly };

        Type[] delegateArgumentTypes = { typeof(Level), typeof(UpdateContext) };

        Type[][] parameterTypeSets =
        {
            new[] {typeof(Level), typeof(UpdateContext)},
            new[] {typeof(UpdateContext)},
            new[] {typeof(Level)},
            Type.EmptyTypes,
        };

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                var validInterface = typeof(ILevelBehavior).IsAssignableFrom(type) && type != typeof(ILevelBehavior) && !type.IsAbstract;

                switch (validInterface)
                {
                    case true when typeof(IGlobalLevelBehavior).IsAssignableFrom(type):
                        RegisterSubBehaviorType(globalSubCache, type, delegateArgumentTypes, parameterTypeSets);
                        break;
                    case true:
                        RegisterSubBehaviorType(levelSubCache, type, delegateArgumentTypes, parameterTypeSets);
                        break;
                }

                TryCreateLevelBehaviorDelegate(type, parameterTypeSets, delegateArgumentTypes);
            }
        }
    }

    public static LevelBehavior CreateLevelBehavior(string? behavior, Level level, UpdateContext context)
    {
        if (behavior == null)
            return new LevelBehavior();

        if (levelCache.TryGetValue(behavior, out var createMethod))
        {
            var levelBehavior = createMethod(level, context);
            InjectSubBehaviors(level, levelBehavior, context);
            return levelBehavior;
        }

        Trace.TraceWarning($"Missing LevelBehaviour \"{behavior}\"");
        return new LevelBehavior();
    }

    private static void TryCreateLevelBehaviorDelegate(Type type, IEnumerable<Type[]> parameterTypeSets, Type[] delegateArgumentTypes)
    {
        if (!typeof(LevelBehavior).IsAssignableFrom(type) || type == typeof(LevelBehavior) || type.IsAbstract)
            return;

        foreach (var parameterTypes in parameterTypeSets)
        {
            var constructor = type.GetConstructor(parameterTypes);
            if (constructor != null)
            {
                var dm = new DynamicMethod($"Create_{type.Name}", typeof(LevelBehavior), delegateArgumentTypes, type);
                var il = dm.GetILGenerator();

                foreach (var parameterType in parameterTypes)
                {
                    if (parameterType == typeof(Level))
                        il.Emit(OpCodes.Ldarg_0);
                    else if (parameterType == typeof(UpdateContext))
                        il.Emit(OpCodes.Ldarg_1);
                    else
                        throw new InvalidOperationException();
                }

                il.Emit(OpCodes.Newobj, constructor);
                il.Emit(OpCodes.Ret);

                levelCache[type.Name] = (CreateLevelBehaviourDelegate)dm.CreateDelegate(typeof(CreateLevelBehaviourDelegate));
                return;
            }
        }

        Trace.TraceWarning($"No valid constructor to create level behavior: {type}");
    }

    private static void RegisterSubBehaviorType(IDictionary<string, CreateLevelSubBehaviorDelegate> cache,
        Type type, Type[] delegateArgumentTypes, Type[][] parameterTypeSets)
    {
        foreach (var parameterTypes in parameterTypeSets)
        {
            var constructor = type.GetConstructor(parameterTypes);
            if (constructor != null)
            {
                var dm = new DynamicMethod("Create_" + type.Name, type, delegateArgumentTypes, type);
                var il = dm.GetILGenerator();

                foreach (var parameterType in parameterTypes)
                    if (parameterType == typeof(Level))
                        il.Emit(OpCodes.Ldarg_0);
                    else if (parameterType == typeof(UpdateContext))
                        il.Emit(OpCodes.Ldarg_1);
                    else
                        throw new InvalidOperationException();

                il.Emit(OpCodes.Newobj, constructor);
                il.Emit(OpCodes.Ret);

                var key = type.Name.Replace("SubBehavior", string.Empty);
                cache[key] = (CreateLevelSubBehaviorDelegate)
                    dm.CreateDelegate(typeof(CreateLevelSubBehaviorDelegate));

                return;
            }
        }

        Trace.TraceWarning($"No valid constructor to create level sub behavior: {type}");
    }

    private static readonly char[] commaSeparator = { ',' };

    private static void InjectSubBehaviors(Level level, LevelBehavior levelBehavior, UpdateContext context)
    {
        var hasGlobalSubBehaviors = globalSubCache is { Count: > 0 };

        var subBehaviorString = level.properties.GetString(Constants.SubBehaviors);
        if (!hasGlobalSubBehaviors && subBehaviorString == null)
        {
            levelBehavior.subBehaviors = noSubBehaviors;
            return;
        }

        string[]? subBehaviors = null;
        if (subBehaviorString != null)
        {
            subBehaviors = subBehaviorString.Split(commaSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (!hasGlobalSubBehaviors && subBehaviors.Length == 0)
            {
                levelBehavior.subBehaviors = noSubBehaviors;
                return;
            }
        }

        var subList = new List<ILevelBehavior>();

        if (hasGlobalSubBehaviors)
            foreach (var globalSubBehaviour in globalSubCache)
                subList.Add(globalSubBehaviour.Value(level, context));

        if (subBehaviors != null)
            foreach (var subBehaviour in subBehaviors)
            {
                if (levelSubCache.TryGetValue(subBehaviour, out var createSubMethod))
                    subList.Add(createSubMethod(level, context));
            }

        levelBehavior.subBehaviors = new ReadOnlyList<ILevelBehavior>(subList);
    }
}