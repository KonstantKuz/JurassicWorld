namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class DeathState : BaseState
        {
            public DeathState(UnitStateMachine stateMachine) : base(stateMachine)
            {
            }

            public override void OnEnterState()
            {
                StateMachine._movementController.IsStopped = true;
            }

            public override void OnTick()
            {
            }

            public override void OnExitState()
            {
            }
        }
    }
}