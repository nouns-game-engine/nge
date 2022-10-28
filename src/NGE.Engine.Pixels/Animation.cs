namespace NGE.Engine.Pixels;

public class Animation
{
    public bool isLooped;
    public List<AnimationFrame> frames = new();

    public int FrameCount => frames.Count;

    public Animation()
    {
        isLooped = true;
    }

    public Animation(bool isLooped)
    {
        this.isLooped = isLooped;
    }
}