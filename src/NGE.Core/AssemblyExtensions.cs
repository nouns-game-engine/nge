using System.Reflection;
using NGE.Core.Assets;

namespace NGE.Core;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetAssetReaderTypes(this Assembly assembly)
    {
        var assetReaderTypes = assembly.GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && t.Implements<IAssetReader>());

        return assetReaderTypes;
    }
}