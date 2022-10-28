using NGE.Engine.StateMachine;

namespace NGE.Engine.Pixel2D;

public class Actor : StateMachine<UpdateContext>
{
    #region State Machine

    public new MethodTable? StateMethods => (MethodTable?) CurrentState?.methodTable;

    public new class MethodTable : StateMachine<UpdateContext>.MethodTable
    {
        public delegate void UpdateDelegate(Actor self, UpdateContext updateContext);

        // ReSharper disable once InconsistentNaming
        public UpdateDelegate Update = null!;
    }

    #endregion

    public virtual void Update(UpdateContext updateContext)
    {
        StateMethods?.Update(this, updateContext);
    }
}