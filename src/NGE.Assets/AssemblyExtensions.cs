using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NGE.Editor;

namespace NGE.Assets;

public static class AssemblyExtensions
{
    public static IEnumerable<Type> GetAssetReaderTypes(this Assembly assembly)
    {
        var assetReaderTypes = assembly.GetTypes()
            .Where(t => !t.IsInterface && !t.IsAbstract && t.Implements<IAssetReader>());

        return assetReaderTypes;
    }
}