using Nouns.Core.StateMachine;
using Xunit;
using Xunit.Abstractions;

namespace Nouns.Tests.StateMachine
{
    public class StateMachineTests
    {
        private readonly ITestOutputHelper console;

        public StateMachineTests(ITestOutputHelper console)
        {
            this.console = console;
        }

        [Fact]
        public void BasicStateTransitions()
        {
            StateProvider.Setup(new[] { typeof(Door).Assembly });

            var updateContext = new UpdateContext();

            var door = new Door(updateContext, console);
            Assert.False(door.IsOpening);
            Assert.False(door.IsOpen);
            Assert.False(door.IsClosing);
            Assert.True(door.IsClosed);
            console.WriteLine("");

            door.Open(updateContext);
            Assert.True(door.IsOpening);
            Assert.False(door.IsOpen);
            Assert.False(door.IsClosing);
            Assert.False(door.IsClosed);
            console.WriteLine("");

            door.Update(updateContext);
            Assert.False(door.IsOpening);
            Assert.True(door.IsOpen);
            Assert.False(door.IsClosing);
            Assert.False(door.IsClosed);
            console.WriteLine("");
            
            door.Close(updateContext);
            Assert.False(door.IsOpening);
            Assert.False(door.IsOpen);
            Assert.True(door.IsClosing);
            Assert.False(door.IsClosed);
            console.WriteLine("");

            door.Update(updateContext);
            Assert.False(door.IsOpening);
            Assert.False(door.IsOpen);
            Assert.False(door.IsClosing);
            Assert.True(door.IsClosed);
        }
    }

    public sealed class UpdateContext { }

    public class GameObject : StateMachine<UpdateContext>
    {
        #region State Machine

        public new MethodTable StateMethods => (MethodTable) CurrentState!.methodTable;

        public new class MethodTable : StateMachine<UpdateContext>.MethodTable
        {
            public delegate void UpdateDelegate(GameObject self, UpdateContext updateContext);
            public UpdateDelegate Update = null!;
        }

        #endregion

        public virtual void Update(UpdateContext updateContext)
        {
            StateMethods.Update(this, updateContext);
        }
    }

    public class Door : GameObject
    {
        private readonly ITestOutputHelper console;

        public bool IsOpening => CurrentState is OpeningState;
        public bool IsOpen => CurrentState is OpenState;
        public bool IsClosing => CurrentState is ClosingState;
        public bool IsClosed => CurrentState is ClosedState;

        public Door(UpdateContext updateContext, ITestOutputHelper console)
        {
            this.console = console;
            SetState<ClosedState>(updateContext);
        }

        public void Open(UpdateContext updateContext)
        {
            console.WriteLine("Door.Open");
            SetState<OpeningState>(updateContext);
        }

        public void Close(UpdateContext updateContext)
        {
            console.WriteLine("Door.Close");
            SetState<ClosingState>(updateContext);
        }

        public override void Update(UpdateContext updateContext)
        {
            console.WriteLine("Door.Update");
            base.Update(updateContext);
        }

        #region Opening

        public class OpeningState : State { }

        private void State_Opening_BeginState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Opening.Begin");
        }

        private void State_Opening_Update(UpdateContext updateContext)
        {
            console.WriteLine("Opening.Update");
            SetState<OpenState>(updateContext);
        }

        private void State_Opening_EndState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Opening.End");
        }

        #endregion

        #region Open

        public class OpenState : State { }

        private void State_Open_BeginState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Open.Begin");
        }

        private void State_Open_Update(UpdateContext updateContext)
        {
            console.WriteLine("Open.Update");
        }

        private void State_Open_EndState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Open.End");
        }

        #endregion

        #region Closing

        public class ClosingState : State { }

        private void State_Closing_BeginState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Closing.Begin");
        }

        private void State_Closing_Update(UpdateContext updateContext)
        {
            console.WriteLine("Closing.Update");
            SetState<ClosedState>(updateContext);
        }

        private void State_Closing_EndState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Closing.End");
        }

        #endregion

        #region Closed

        protected class ClosedState : State { }

        private void State_Closed_BeginState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Closed.Begin");
        }

        private void State_Closed_Update(UpdateContext updateContext)
        {
            console.WriteLine("Closed.Update");
        }

        private void State_Closed_EndState(UpdateContext updateContext, State previousState)
        {
            console.WriteLine("Closed.End");
        }

        #endregion
    }
}
