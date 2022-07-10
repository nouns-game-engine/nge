using Microsoft.Extensions.FileProviders;
using Nouns.CLI.Configuration;
using Tomlyn;

// ReSharper disable once CheckNamespace (following convention)
namespace Microsoft.Extensions.Configuration
{
    public static class TomlConfigurationExtensions
    {
        public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, TomlParserOptions parserOptions = TomlParserOptions.ParseAndValidate)
        {
            return AddTomlFile(builder, null!, path, false, false, parserOptions);
        }

        public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional, TomlParserOptions parserOptions = TomlParserOptions.ParseAndValidate)
        {
            return AddTomlFile(builder, null!, path, optional, false, parserOptions);
        }

        public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, string path, bool optional,
            bool reloadOnChange, TomlParserOptions parserOptions = TomlParserOptions.ParseAndValidate)
        {
            return AddTomlFile(builder, null!, path, optional, reloadOnChange, parserOptions);
        }

        public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder, IFileProvider provider,
            string path, bool optional, bool reloadOnChange, TomlParserOptions parserOptions = TomlParserOptions.ParseAndValidate)
        {
            return builder.AddTomlFile(source =>
            {
                source.Optional = optional;
                source.ReloadOnChange = reloadOnChange;
                source.FileProvider = provider;
                source.Path = path;
                source.ResolveFileProvider();
                source.ParserOptions = parserOptions;
            });
        }

        public static IConfigurationBuilder AddTomlFile(this IConfigurationBuilder builder,
            Action<TomlConfigurationSource> configureAction)
        {
            return builder.Add(configureAction);
        }
    }
}