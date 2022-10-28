using System.IO;
using NGE.Core.Serialization;
using NGE.Engine.Pixel3D;
using NGE.Engine.Pixel3D.Serialization;
using NGE.Engine.Pixels;
using NGE.Engine.Pixels.Serialization;
using NGE.Graphics;
using Xunit;

namespace NGE.Tests.Serialization
{
    public sealed class RoundTripTests
    {
        [Fact]
        public void AnimationSets()
        {
            var animationSet = ManualRoundTripAnimationSet();

            RoundTrip.Check<AnimationSet, AnimationSerializeContext, AnimationDeserializeContext>(animationSet, Headless.AcquireServices());
        }

        [Fact]
        public void Levels()
        {
            var level = ManualRoundTripLevel();

            RoundTrip.Check<Level, LevelSerializeContext, LevelDeserializeContext>(level, Headless.AcquireServices());
        }

        private static AnimationSet ManualRoundTripAnimationSet()
        {
            var services = Headless.AcquireServices();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new AnimationSerializeContext(firstBinaryWriter, services);

            var animationSet = SerializationTestFactory.CreateFakeAnimationSet();
            animationSet.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new AnimationDeserializeContext(br, services);
            var deserialized = new AnimationSet(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new AnimationSerializeContext(secondBinaryWriter, services);
            deserialized.Serialize(secondSerializeContext);
            return animationSet;
        }

        private static Level ManualRoundTripLevel()
        {
            var services = Headless.AcquireServices();

            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = new LevelSerializeContext(firstBinaryWriter, services);

            var level = SerializationTestFactory.CreateFakeLevel();
            level.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = new LevelDeserializeContext(br, services);
            var deserialized = new Level(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = new LevelSerializeContext(secondBinaryWriter, services);
            deserialized.Serialize(secondSerializeContext);

            return level;
        }
    }
}
