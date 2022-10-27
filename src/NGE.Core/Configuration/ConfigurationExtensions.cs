using Microsoft.Extensions.Configuration;

namespace NGE.Core.Configuration;

public static class ConfigurationExtensions
{
    public static void Set(this IConfiguration configuration, string key, string value)
    {
        if(configuration is IConfigurationRoot root)
            root.Set(key, value);
        else
        {
            throw new InvalidOperationException("Tried to add a configuration value on a non-root container");
        }
    }

    public static void Set(this IConfigurationRoot configuration, string key, string value)
    {
        foreach (var provider in configuration.Providers)
        {
            if (provider is TomlConfigurationProvider toml)
            {
                toml.Set(key, value);
            }
        }
    }
}