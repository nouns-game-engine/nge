using NGE.Core.Serialization;
using NGE.Engine.Pixel3D.Serialization;
using NGE.Engine.Pixels;

namespace NGE.Engine.Pixel3D;

public class LevelObject : ISerialize<LevelSerializeContext>, IDeserialize<LevelDeserializeContext>
{
    public AnimationSet AnimationSet { get; set; } = null!;
    public Position Position { get; set; }
    public bool FacingLeft { get; set; }

    public readonly Dictionary<string, string> properties = new();

    // ReSharper disable once UnusedMember.Global (Serialization)
    public LevelObject() { }

    public LevelObject(AnimationSet animationSet, Position position, bool facingLeft)
    {
        AnimationSet = animationSet;
        Position = position;
        FacingLeft = facingLeft;
    }

    public LevelObject(LevelDeserializeContext context)
    {
        Deserialize(context);
    }

    public void Deserialize(LevelDeserializeContext context)
    {
        AnimationSet = context.ReadAnimationSet();
        Position = context.br.ReadPosition();
        FacingLeft = context.br.ReadBoolean();

        var propertyCount = context.br.ReadInt32();
        for (var i = 0; i < propertyCount; i++)
        {
            properties.Add(context.br.ReadString(), context.br.ReadString());
        }
    }
    
    public void Serialize(LevelSerializeContext context)
    {
        context.WriteAnimationSet(AnimationSet);

        context.bw.Write(Position);
        context.bw.Write(FacingLeft);

        context.bw.Write(properties.Count);
        foreach (var kvp in properties)
        {
            context.bw.Write(kvp.Key);
            context.bw.Write(kvp.Value);
        }
    }

    public void WriteToFile(string path, IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

    public void ReadFromFile(string path, IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }

}