namespace Nouns.Engine.Pixel2D;

public class Thing
{
    public AnimationSet AnimationSet { get; set; } = null!;
    public Position Position { get; set; }
    public bool FacingLeft { get; set; }

    public Thing() { }

    public Thing(AnimationSet animationSet, Position position, bool facingLeft)
    {
        AnimationSet = animationSet;
        Position = position;
        FacingLeft = facingLeft;
    }
}