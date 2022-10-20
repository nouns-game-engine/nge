using NGE.Engine.Pixel2D;
using Tomlyn;
using Xunit;
using Xunit.Abstractions;

namespace NGE.Tests.Serialization
{
    public class TomlSerializationTests
    {
        private readonly ITestOutputHelper console;

        public TomlSerializationTests(ITestOutputHelper console) => this.console = console;

        [Fact]
        public void NestedObjectTests()
        {
            var definitions = new PixelsDefinitions();
            var fakeLevel = SerializationTestFactory.CreateFakeLevel();
            definitions.levels.Add(fakeLevel);

            var toml = ToToml(definitions);
            console.WriteLine(toml);

            var definitionsFromToml = FromToml(toml);
            Assert.Equal(definitionsFromToml.levels[0].friendlyName, fakeLevel.friendlyName);
        }

        private static string ToToml(PixelsDefinitions definitions)
        {
            var options = new TomlModelOptions { IgnoreMissingProperties = false, IncludeFields = true };
            Assert.True(Toml.TryFromModel(definitions, out var toml, out var diagnostics, options));
            Assert.False(diagnostics.HasErrors);
            return toml;
        }

        private static PixelsDefinitions FromToml(string toml)
        {
            var options = new TomlModelOptions { IgnoreMissingProperties = false, IncludeFields = true };
            Assert.True(Toml.TryToModel<PixelsDefinitions>(toml, out var definitions, out var diagnostics, null, options));
            Assert.NotNull(diagnostics);
            Assert.False(diagnostics.HasErrors);
            return definitions;
        }
    }
}
