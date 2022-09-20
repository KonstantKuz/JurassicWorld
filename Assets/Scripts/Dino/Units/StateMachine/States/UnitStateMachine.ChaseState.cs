using Dino.Units.Component.Target;
using Dino.Units.Enemy.Model.EnemyAttack;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        protected class ChaseState : BaseState
        {
            private readonly EnemyAttackModel _attackModel;
            
            private Unit Owner => StateMachine._owner;
            private ITarget Target => StateMachine._targetProvider.Target;
            private Vector3 TargetPosition => Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            protected bool IsTargetBlocked =>
                Physics.Linecast(Owner.SelfTarget.Center.position, Target.Center.position, StateMachine._layerMaskProvider.ObstacleMask);

            public ChaseState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = Owner.RequireEnemyModel();
                _attackModel = enemyModel.AttackModel;
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
                if (DistanceToTarget < _attackModel.AttackDistance && !IsTargetBlocked)
                {
                    StateMachine.SetState(UnitState.Attack);
                    return;
                }

                StateMachine._movementController.MoveTo(TargetPosition);
            }
        }
    }
}