using Nouns.Engine.Pixel2D.Serialization;
using Nouns.Engine.Pixel2D;
using System.IO;
using NGE.Core.Serialization;
using Nouns.Graphics.Core;
using Xunit;

namespace NGE.Tests.Serialization
{
    public sealed class RoundTripTests
    {
        [Fact]
        public void AnimationSets()
        {
            var graphicsDevice = Headless.AcquireGraphicsDevice();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new AnimationSerializeContext(firstBinaryWriter);

            var animationSet = SerializationTestFactory.CreateFakeAnimationSet();
            animationSet.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new AnimationDeserializeContext(br, graphicsDevice);
            var deserialized = new AnimationSet(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new AnimationSerializeContext(secondBinaryWriter);
            deserialized.Serialize(secondSerializeContext);
        }

        [Fact]
        public void Levels()
        {
            var graphicsDevice = Headless.AcquireGraphicsDevice();
            var assetPathProvider = Headless.AcquireAssetManager();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new LevelSerializeContext(firstBinaryWriter, assetPathProvider);

            var level = SerializationTestFactory.CreateFakeLevel();
            level.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new LevelDeserializeContext(br, assetPathProvider, graphicsDevice);
            var deserialized = new Level(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new LevelSerializeContext(secondBinaryWriter, assetPathProvider);
            deserialized.Serialize(secondSerializeContext);
        }
    }
}
