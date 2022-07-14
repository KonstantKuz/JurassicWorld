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
            private readonly ITargetProvider _targetProvider;
            
            private Unit Owner => StateMachine._owner;

            private Vector3 TargetPosition => _targetProvider.Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            private float OwnerSize => Owner.transform.lossyScale.x * Owner.Bounds.extents.x;
            
            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
                _targetProvider = Owner.gameObject.RequireComponent<ITargetProvider>();
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
                Debug.DrawRay(Owner.transform.position + Vector3.up, Owner.transform.forward * OwnerSize, Color.red);
                
                if (DistanceToTarget < _attackModel.AttackDistance + OwnerSize)
                {
                    StateMachine.SetState(UnitState.Attack);
                    return;
                }

                StateMachine._movementController.MoveTo(TargetPosition);
            }
        }
    }
}