using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class IdleState : BaseState
        {
            private Unit Owner => StateMachine._owner;
            public IdleState(UnitStateMachine stateMachine) : base(stateMachine)
            {
            }            

            public override void OnEnterState()
            {
                StateMachine._movementController.IsStopped = true;
                StateMachine._animationWrapper.PlayIdleSmooth();
                Owner.Damageable.OnDamageTaken += StateMachine.LookTowardsDamage;
            }

            public override void OnExitState()
            {
                Owner.Damageable.OnDamageTaken -= StateMachine.LookTowardsDamage;
            }

            public override void OnTick()
            {
                StateMachine.ChaseTargetIfExists();
            }
        }
    }
}