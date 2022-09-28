using Microsoft.Extensions.Configuration;
using Tomlyn;

namespace Nouns.Core.Configuration
{
    public sealed class TomlConfigurationSource : FileConfigurationSource
    {
        public TomlParserOptions ParserOptions { get; set;  } = TomlParserOptions.ParseAndValidate;

        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new TomlConfigurationProvider(this);
        }
    }
}