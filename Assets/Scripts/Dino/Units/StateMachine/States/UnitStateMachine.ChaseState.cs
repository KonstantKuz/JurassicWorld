using Dino.Units.Component.Target;
using Dino.Units.Enemy.Model;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        protected class ChaseState : BaseState
        {
            private readonly EnemyAttackModel _attackModel;
            private Vector3 _lastTargetPosition;
            
            private Unit Owner => StateMachine._owner;
            private ITarget Target => StateMachine._targetProvider.Target;            

            private Vector3 TargetPosition => Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);

            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = Owner.Model as EnemyUnitModel;
                if (enemyModel == null)
                {
                    this.Logger().Error("Unit model must be EnemyUnitModel");
                    return;
                }
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
                if (DistanceToTarget < _attackModel.AttackDistance)
                {
                    StateMachine.SetState(UnitState.Attack);
                    return;
                }

                StateMachine._movementController.MoveTo(TargetPosition);
            }

            private void UpdateLastTargetPosition()
            {
                if (!Target.IsTargetValidAndAlive()) return;
                _lastTargetPosition = TargetPosition;
            }
        }
    }
}