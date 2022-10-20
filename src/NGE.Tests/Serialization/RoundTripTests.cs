using System.IO;
using NGE.Core.Serialization;
using NGE.Engine.Pixel2D;
using NGE.Engine.Pixel2D.Serialization;
using NGE.Graphics;
using Xunit;

namespace NGE.Tests.Serialization
{
    public sealed class RoundTripTests
    {
        [Fact]
        public void AnimationSets()
        {
            var services = Headless.AcquireServices();
            var animationSet = ManualRoundTripAnimationSet();
            RoundTrip.Check<AnimationSet, AnimationSerializeContext, AnimationDeserializeContext>(animationSet, services);
        }

        private static AnimationSet ManualRoundTripAnimationSet()
        {
            var services = Headless.AcquireServices();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new AnimationSerializeContext(firstBinaryWriter);

            var animationSet = SerializationTestFactory.CreateFakeAnimationSet();
            animationSet.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new AnimationDeserializeContext(br, services);
            var deserialized = new AnimationSet(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new AnimationSerializeContext(secondBinaryWriter);
            deserialized.Serialize(secondSerializeContext);
            return animationSet;
        }

        [Fact]
        public void Levels()
        {
            var services = Headless.AcquireServices();
            var assetPathProvider = Headless.AcquireAssetManager();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new LevelSerializeContext(firstBinaryWriter, assetPathProvider);

            var level = SerializationTestFactory.CreateFakeLevel();
            level.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new LevelDeserializeContext(br, services);
            var deserialized = new Level(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new LevelSerializeContext(secondBinaryWriter, assetPathProvider);
            deserialized.Serialize(secondSerializeContext);
        }
    }
}
