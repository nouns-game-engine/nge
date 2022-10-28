namespace NGE.Engine.Pixels;

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

    public AnimationFrame? CurrentFrame => animation?.frames[frame];

    public void Tick()
    {
        if (animation == null)
            return;

        tick += 1;
        var frameDelay = animation.frames[frame].delay;
        if (frameDelay > 0)
        {
            if (tick >= frameDelay)
                AdvanceFrame();
        }
    }

    public void AdvanceFrame()
    {
        tick = 0;
        frame++;

        if (frame >= animation.FrameCount)
        {
            if (animation.isLooped)
            {
                frame = 0;
            }
            else
            {
                frame = animation.FrameCount - 1;
                tick = animation.frames[frame].delay;
            }
        }
    }
}