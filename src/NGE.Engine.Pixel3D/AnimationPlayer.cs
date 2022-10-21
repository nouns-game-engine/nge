namespace NGE.Engine.Pixel3D;

public struct AnimationPlayer
{
    private readonly Animation animation;
    public int frame;
    public int tick;

    public AnimationPlayer(Animation animation)
    {
        this.animation = animation;
        this.frame = 0;
        this.tick = 0;
    }

    public AnimationFrame CurrentFrame => animation.frames[frame];
}