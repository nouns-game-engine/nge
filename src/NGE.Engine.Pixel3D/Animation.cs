namespace NGE.Engine.Pixel3D;

public class Animation
{
    public bool isLooped;
    public List<AnimationFrame> frames = new();

    public Animation()
    {
        isLooped = true;
    }

    public Animation(bool isLooped)
    {
        this.isLooped = isLooped;
    }
}