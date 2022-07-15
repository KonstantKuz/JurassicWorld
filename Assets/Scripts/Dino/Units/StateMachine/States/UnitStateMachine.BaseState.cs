namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        protected abstract class BaseState
        {
            protected readonly UnitStateMachine StateMachine;

            protected BaseState(UnitStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }

            public abstract void OnEnterState();
            public abstract void OnTick();
            public abstract void OnExitState();
        }
    }
}