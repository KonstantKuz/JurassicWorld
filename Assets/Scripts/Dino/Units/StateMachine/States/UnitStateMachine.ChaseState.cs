using Dino.Units.Component.Target;
using Dino.Units.Enemy.Model.EnemyAttack;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        protected class ChaseState : BaseState
        {
            private const float ATTACK_DISTANCE_COEFF = 0.75f;
            
            private readonly EnemyAttackModel _attackModel;
            private Vector3 _lastTargetPosition;
            
            private Unit Owner => StateMachine._owner;
            private ITarget Target => StateMachine._targetProvider.Target;
            private Vector3 TargetPosition => Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            protected bool IsTargetBlocked =>
                Physics.Linecast(Owner.SelfTarget.Center.position, Target.Center.position, StateMachine._layerMaskProvider.ObstacleMask);
            private Vector3 ChasePoint
            {
                get
                {
                    var pos = StateMachine.transform.position;
                    var vectorToTarget = TargetPosition - pos;
                    var chasePoint = pos +
                                     (vectorToTarget.magnitude - ATTACK_DISTANCE_COEFF * _attackModel.AttackDistance) *
                                     vectorToTarget.normalized;
                    return chasePoint;
                }
            }
            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = Owner.RequireEnemyModel();
                _attackModel = enemyModel.AttackModel;
            }            

            public override void OnEnterState()
            {
                UpdateLastTargetPosition();
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                UpdateLastTargetPosition();
                if (!Target.IsTargetValidAndAlive())
                {
                    StateMachine.SetState(UnitState.LookAround, _lastTargetPosition);
                    return;
                }
                if (DistanceToTarget < _attackModel.AttackDistance && !IsTargetBlocked)
                {
                    StateMachine.SetState(UnitState.Attack);
                    return;
                }
                
                StateMachine._movementController.MoveTo(ChasePoint);
            }

            private void UpdateLastTargetPosition()
            {
                if (!Target.IsTargetValidAndAlive()) return;
                _lastTargetPosition = TargetPosition;
            }
        }
    }
}