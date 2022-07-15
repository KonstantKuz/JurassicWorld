using Dino.Extension;
using Dino.Units.Model;
using Dino.Units.Target;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        protected class ChaseState : BaseState
        {
            private readonly IAttackModel _attackModel;
            
            private Unit Owner => StateMachine._owner;

            private Vector3 TargetPosition => StateMachine._targetProvider.Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            
            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
            }            

            public override void OnEnterState()
            {
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                if (DistanceToTarget < _attackModel.AttackDistance)
                {
                    StateMachine.SetState(UnitState.Attack);
                    return;
                }

                StateMachine._movementController.MoveTo(TargetPosition);
            }
        }
    }
}