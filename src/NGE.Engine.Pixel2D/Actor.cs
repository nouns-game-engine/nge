using NGE.Engine.Pixels;
using NGE.Engine.StateMachine;

// ReSharper disable PossiblyImpureMethodCallOnReadonlyVariable (Mutable Struct)

namespace NGE.Engine.Pixel2D;

public class Actor : StateMachine<UpdateContext>
{
    public Position position;

    public Actor() { }

    public Actor(AnimationSet animationSet)
    {
        this.animationSet = animationSet;
        currentAnimation = new AnimationPlayer(animationSet.animations[0]);
    }

    public virtual void Update(UpdateContext updateContext)
    {
        TickAnimation(updateContext);

        StateMethods?.Update(this, updateContext);
    }

    public virtual void Draw(DrawContext drawContext)
    {
        currentAnimation.CurrentFrame?.Draw(drawContext, position, false);
    }

    #region Animation

    public AnimationSet animationSet = null!;
    protected AnimationPlayer currentAnimation;

    private void TickAnimation(UpdateContext updateContext)
    {
        currentAnimation.Tick();
    }

    #endregion

    #region State Machine

    public new MethodTable? StateMethods => (MethodTable?)CurrentState?.methodTable;

    public new class MethodTable : StateMachine<UpdateContext>.MethodTable
    {
        public delegate void UpdateDelegate(Actor self, UpdateContext updateContext);

        // ReSharper disable once InconsistentNaming
        public UpdateDelegate Update = null!;
    }

    #endregion
}