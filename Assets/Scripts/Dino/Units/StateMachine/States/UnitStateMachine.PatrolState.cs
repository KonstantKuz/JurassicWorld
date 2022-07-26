using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Enemy.Model;
using Dino.Units.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class PatrolState : BaseState
        {
            private const float PRECISION_DISTANCE = 0.2f;

            private readonly PatrolStateModel _stateModel;
            
            private PatrolPathProvider _pathProvider;

            private Unit Owner => StateMachine._owner;
            private PatrolPathProvider PathProvider => _pathProvider ??= Owner.gameObject.RequireComponent<PatrolPathProvider>();
            private bool HasPath => PathProvider.PatrolPath != null;
            [CanBeNull]
            private Transform NextPathPoint { get; set; }
            private float DistanceToPathPoint => Vector3.Distance(Owner.transform.position, NextPathPoint.position);
            
            public PatrolState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _stateModel = Owner.RequireEnemyModel().PatrolStateModel;
            }
            
            public override void OnEnterState()
            {
                if (!HasPath) return;
                
                GoToNextPoint();
                Owner.Damageable.OnDamageTaken += StateMachine.LookAround;
            }
            
            public override void OnExitState()
            {
                Owner.Damageable.OnDamageTaken -= StateMachine.LookAround;
            }

            public override void OnTick()
            {
                StateMachine.ChaseTargetIfExists();
                PathProvider.IsLastGivenPointVisited = DistanceToPathPoint < PRECISION_DISTANCE;
                if (!HasPath || !PathProvider.IsLastGivenPointVisited)
                {
                    return;
                }
                StateMachine.Wait(_stateModel.PatrolIdleTime, OnWaitTimeEnd);
            }

            private void OnWaitTimeEnd()
            {
                GoToNextPoint();
                TryResetWaitTimer();
            }
            
            private void GoToNextPoint()
            {
                NextPathPoint = PathProvider.Pop();
                StateMachine._movementController.MoveTo(NextPathPoint.position);
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
            }

            private void TryResetWaitTimer()
            {
                if (PathProvider.PatrolPath.IsEndOfPath(NextPathPoint))
                {
                    StateMachine._waitTimer = 0f;
                }
            }
        }
    }
}