namespace Nouns.Graphics.Pixels;

public class Animation
{
    private readonly bool isLooped;

    public Animation(bool isLooped = true)
    {
        this.isLooped = isLooped;
        Frames = new List<AnimationFrame>();
    }

    public List<AnimationFrame> Frames { get; set; }
}