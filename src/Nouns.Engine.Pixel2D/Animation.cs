namespace Nouns.Engine.Pixel2D;

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