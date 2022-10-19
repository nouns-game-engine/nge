namespace Nouns.Engine.Pixel2D;

public class Animation
{
    public bool isLooped;
    public List<AnimationFrame> frames = new();

    public Animation()
    {
        this.isLooped = false;
    }

    public Animation(bool isLooped = true)
    {
        this.isLooped = isLooped;
    }
}