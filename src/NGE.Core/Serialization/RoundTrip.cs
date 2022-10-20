namespace NGE.Core.Serialization
{
    public sealed class RoundTrip
    {
        public static void Check<TRoundTrip, TSerializeContext, TDeserializeContext>(TRoundTrip toSerialize, IServiceProvider serviceProvider)
            where TRoundTrip : ISerialize<TSerializeContext>, IDeserialize<TDeserializeContext>, new()
            where TSerializeContext : ISerializeContext
            where TDeserializeContext : IDeserializeContext
        {
            var firstMemoryStream = new MemoryStream();
            var firstBinaryWriter = new BinaryWriter(firstMemoryStream);
            var firstSerializeContext = (TSerializeContext) Activator.CreateInstance(typeof(TSerializeContext), firstBinaryWriter, serviceProvider)!;

            toSerialize.Serialize(firstSerializeContext);
            var originalData = firstMemoryStream.ToArray();

            var br = new BinaryReader(new MemoryStream(originalData));
            var deserializeContext = (TDeserializeContext) Activator.CreateInstance(typeof(TDeserializeContext), br, serviceProvider)!;
            var deserialized = new TRoundTrip();
            deserialized.Deserialize(deserializeContext);

            var secondMemoryStream = new MemoryCompareStream(originalData);
            var secondBinaryWriter = new BinaryWriter(secondMemoryStream);
            var secondSerializeContext = (TSerializeContext)Activator.CreateInstance(typeof(TSerializeContext), secondBinaryWriter, serviceProvider)!;
            deserialized.Serialize(secondSerializeContext);
        }
    }
}
