namespace NGE.Engine.Pixel2D;

public interface ILevelBehavior
{
    void BeforeBeginLevel(UpdateContext updateContext);
    void BeginLevelTriggers(UpdateContext updateContext);
    void BeginLevel(UpdateContext updateContext, Level previousLevel, string targetSpawn);

    void BeforeUpdate(UpdateContext updateContext);
    void AfterUpdate(UpdateContext updateContext);
    void BeforeBackgroundDraw(DrawContext drawContext);
    void AfterDraw(DrawContext drawContext);
}