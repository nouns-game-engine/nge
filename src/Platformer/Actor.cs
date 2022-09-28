using Nouns.Engine.Pixels;
using Nouns.StateMachine;

namespace Platformer
{
    public class Actor : StateMachine<UpdateContext>
    {
        public AnimationSet animationSet;
        public Position position;
        public bool facingLeft;

        private readonly AnimationPlayer currentAnimation;

        public Actor(Thing thing, UpdateContext updateContext)
        {
            animationSet = thing.AnimationSet;
            position = thing.Position;
            facingLeft = thing.FacingLeft;
            currentAnimation = new AnimationPlayer(animationSet.animations[0]);
        }
        
        public void Update(UpdateContext updateContext)
        {
            StateMethods.Update(this, updateContext);
        }

        public void Draw(DrawContext drawContext)
        {
            currentAnimation.CurrentFrame.Draw(drawContext, position, facingLeft);
        }

        #region State Machine

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
