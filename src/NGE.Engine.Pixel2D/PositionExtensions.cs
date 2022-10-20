namespace NGE.Engine.Pixel2D;

public static class PositionExtensions
{
    public static void Write(this BinaryWriter bw, Position position)
    {
        bw.Write(position.x);
        bw.Write(position.y);
        bw.Write(position.z);
    }

    public static Position ReadPosition(this BinaryReader br)
    {
        Position p;
        p.x = br.ReadInt32();
        p.y = br.ReadInt32();
        p.z = br.ReadInt32();
        return p;
    }
}