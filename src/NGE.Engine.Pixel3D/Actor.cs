using NGE.Editor;
using NGE.Engine.StateMachine;

namespace NGE.Engine.Pixel3D
{
    public class Actor : StateMachine<UpdateContext>
    {
        public AnimationSet animationSet;
        public Position position;
        public bool facingLeft;

        private readonly AnimationPlayer currentAnimation;

        public Actor(LevelObject levelObject, UpdateContext updateContext)
        {
            animationSet = levelObject.AnimationSet;
            position = levelObject.Position;
            facingLeft = levelObject.FacingLeft;
            currentAnimation = new AnimationPlayer(animationSet.animations[0]);
        }
        
        public void Update(PixelsUpdateContext updateContext)
        {
            StateMethods.Update(this, updateContext);
        }

        public void Draw(PixelsDrawContext drawContext)
        {
            currentAnimation.CurrentFrame.Draw(drawContext, position, facingLeft);
        }

        #region State Machine

        [NonEditable]
        public new MethodTable StateMethods => (MethodTable)CurrentState!.methodTable;

        public new class MethodTable : StateMachine<UpdateContext>.MethodTable
        {
            public delegate void UpdateDelegate(Actor self, UpdateContext updateContext);

            // ReSharper disable once InconsistentNaming
            public UpdateDelegate Update = null!;
        }

        #endregion
    }
}
