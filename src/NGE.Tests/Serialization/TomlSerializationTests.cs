using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nouns.Engine.Pixel2D;
using Nouns.Graphics.Core;
using Tomlyn;
using Xunit;
using Xunit.Abstractions;

namespace NGE.Tests.Serialization
{
    public class TomlSerializationTests
    {
        private readonly ITestOutputHelper console;

        public TomlSerializationTests(ITestOutputHelper console)
        {
            Bootstrap.Init();

            this.console = console;
        }

        [Fact]
        public void NestedObjectTests()
        {
            var definitions = new PixelsDefinitions();

            var graphicsDevice = HeadlessGraphicsDeviceService.AcquireGraphicsDevice(out _);
            
            var texture = new Texture2D(graphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });

            var sprite = new Sprite(texture);
            
            var cel = new Cel(sprite);
            
            var frame = new AnimationFrame();
            frame.layers.Add(cel);
            
            var animation = new Animation();
            animation.frames.Add(frame);

            var animationSet = new AnimationSet();
            animationSet.friendlyName = "fake";
            animationSet.animations.Add(animation);

            var position = new Position(100, 100);
            var thing = new LevelObject(animationSet, position, false);

            var level = new Level();
            level.levelObjects.Add(thing);
            definitions.levels.Add(level);

            var toml = ToToml(definitions);
            console.WriteLine(toml);

            var definitionsFromToml = FromToml(toml);
            Assert.Equal(definitionsFromToml.levels[0].levelObjects[0].AnimationSet.friendlyName, animationSet.friendlyName);
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
