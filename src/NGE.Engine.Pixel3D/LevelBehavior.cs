using NGE.Engine.StateMachine;

namespace NGE.Engine.Pixel3D;

public class LevelBehavior : ILevelBehavior
{
    public ReadOnlyList<ILevelBehavior> subBehaviors = new();

    public virtual void BeforeBeginLevel(UpdateContext updateContext)
    {
        foreach(var subBehaviour in subBehaviors)
            subBehaviour.BeforeBeginLevel(updateContext);
    }

    public virtual void BeginLevelTriggers(UpdateContext updateContext)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.BeginLevelTriggers(updateContext);
    }

    public virtual void BeginLevel(UpdateContext updateContext, Level previousLevel, string targetSpawn)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.BeginLevel(updateContext, previousLevel, targetSpawn);
    }

    public virtual void BeforeUpdate(UpdateContext updateContext)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.BeforeUpdate(updateContext);
    }

    public virtual void AfterUpdate(UpdateContext updateContext)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.AfterUpdate(updateContext);
    }

    public virtual void BeforeBackgroundDraw(DrawContext drawContext)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.BeforeBackgroundDraw(drawContext);
    }

    public virtual void AfterDraw(DrawContext drawContext)
    {
        foreach (var subBehaviour in subBehaviors)
            subBehaviour.AfterDraw(drawContext);
    }
}