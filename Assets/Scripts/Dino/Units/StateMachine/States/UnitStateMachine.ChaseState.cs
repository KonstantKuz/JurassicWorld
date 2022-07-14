using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class ChaseState : BaseState
        {
            private IAttackModel _attackModel;
            private Unit Owner => StateMachine._owner;

            private Vector3 TargetPosition => StateMachine.Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
            }            

            public override void OnEnterState()
            {
                StateMachine._agent.isStopped = false;
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                if (DistanceToTarget < _attackModel.AttackDistance + StateMachine._agent.radius)
                {
                    StateMachine.SetState(new AttackState(StateMachine));
                    return;
                }

                StateMachine._agent.SetDestination(TargetPosition);
            }
        }
    }
}