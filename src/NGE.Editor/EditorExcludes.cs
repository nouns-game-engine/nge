using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace NGE.Editor;

public sealed class EditorExcludes
{
    private readonly HashSet<string> names = new(StringComparer.OrdinalIgnoreCase);
    private readonly HashSet<string> wildcards = new(StringComparer.OrdinalIgnoreCase);

    public bool IsExcluded(string dllName)
    {
        var excluded = names.Contains(dllName);

        if (!excluded)
        {
            foreach (var excludeWildcard in wildcards)
            {
                if (!dllName.StartsWith(excludeWildcard, StringComparison.OrdinalIgnoreCase))
                    continue;
                excluded = true;
                break;
            }
        }

        return excluded;
    }

    public static EditorExcludes FromConfiguration(IConfiguration configuration)
    {
        var excludes = new EditorExcludes();
            
        var excludedAssembliesString = configuration.GetSection("editor")["excludeAssemblies"];
        if (!string.IsNullOrWhiteSpace(excludedAssembliesString))
        {
            var assemblyStrings = excludedAssembliesString.Split(";", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var assemblyString in assemblyStrings)
            {
                if (assemblyString.EndsWith("*"))
                    excludes.wildcards.Add(assemblyString[..^1]);
                else
                    excludes.names.Add(assemblyString);
            }
        }

        return excludes;
    }
}