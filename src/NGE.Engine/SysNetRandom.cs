namespace NGE.Engine;

internal sealed class SysNetRandom : IRandomProvider
{
    private readonly Random random;

    public SysNetRandom()
    {
        random = new Random();
    }

    public float NextFloat()
    {
        return random.NextSingle();
    }
}