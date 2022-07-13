namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private abstract class BaseState
        {
            protected readonly StateMachine.UnitStateMachine StateMachine;

            protected BaseState(StateMachine.UnitStateMachine stateMachine)
            {
                StateMachine = stateMachine;
            }

            public abstract void OnEnterState();
            public abstract void OnTick();
            public abstract void OnExitState();
        }
    }
}