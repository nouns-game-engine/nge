namespace Nouns.Engine.Pixels;

public class Thing
{
    public AnimationSet AnimationSet { get; set; }
    public Position Position { get; set; }
    public bool FacingLeft { get; set; }

    public Thing(AnimationSet animationSet, Position position, bool facingLeft)
    {
        AnimationSet = animationSet;
        Position = position;
        FacingLeft = facingLeft;
    }
}