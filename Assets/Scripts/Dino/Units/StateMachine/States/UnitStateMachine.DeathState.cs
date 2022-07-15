using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class DeathState : BaseState
        {
            private readonly int _deathHash = Animator.StringToHash("Death");

            public DeathState(UnitStateMachine stateMachine) : base(stateMachine)
            {
            }

            public override void OnEnterState()
            {
                StateMachine._movementController.IsStopped = true;
                StateMachine._animator.SetTrigger(_deathHash);
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