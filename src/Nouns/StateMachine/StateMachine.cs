using System;
using System.Diagnostics.CodeAnalysis;

namespace Nouns.StateMachine
{
    public class StateMachine<TUpdateContext> : StateProvider
    {
        public StateMachine()
        {
            CurrentState = GetState<State>();
        }

        public MethodTable? StateMethods => (MethodTable?) CurrentState?.methodTable;

        public State? CurrentState { get; private set; }

        public override string ToString()
        {
            return $"{GetType().Name} ({(CurrentState != null ? CurrentState.GetType().Name : "(null)")})";
        }

        public void SetState<TState>(TUpdateContext updateContext, bool allowStateRestart = false) where TState : State, new()
        {
            _DirectlySetState(GetState<TState>(), updateContext, allowStateRestart);
        }
		
        /// <summary>Set a state from a previously found state object. Not for general use.</summary>
        [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Underscore used to emphasize the method should not be used normally.")]
        public void _DirectlySetState(State nextState, TUpdateContext updateContext, bool allowStateRestart)
        {
            if (!allowStateRestart && ReferenceEquals(CurrentState, nextState))
                return; // Don't re-enter the same state

            if(StateMethods?.EndState != null)
                StateMethods.EndState(this, updateContext, nextState);

            var previousState = CurrentState;
            CurrentState = nextState;

            if(StateMethods?.BeginState != null)
                StateMethods?.BeginState.Invoke(this, updateContext, previousState!);
        }


        public new class MethodTable : StateProvider.MethodTable
        {
            [AlwaysNullChecked] public Action<StateMachine<TUpdateContext>, TUpdateContext, State> BeginState = null!;

            [AlwaysNullChecked] public Action<StateMachine<TUpdateContext>, TUpdateContext, State> EndState = null!;
        }
    }
}