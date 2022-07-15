using Dino.Extension;
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
            
            private float _waitTimer;
            private PatrolPathProvider _pathProvider;

            private Unit Owner => StateMachine._owner;
            private PatrolPathProvider PathProvider => _pathProvider ??= Owner.gameObject.RequireComponent<PatrolPathProvider>();
            private bool HasPath => PathProvider.PatrolPath != null;
            [CanBeNull]
            private Transform NextPathPoint { get; set; }
            private float DistanceToPathPoint => Vector3.Distance(Owner.transform.position, NextPathPoint.position);
            private bool IsTimeToGo => _waitTimer >= _stateModel.PatrolIdleTime;
            
            public PatrolState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = (EnemyUnitModel) Owner.Model;
                Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
                _stateModel = enemyModel.PatrolStateModel;
            }
            
            public override void OnEnterState()
            {
                if (!HasPath) return;
                
                GoToNextPoint();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                if (StateMachine._targetProvider.Target != null)
                {
                    StateMachine.SetState(UnitState.Chase);
                }
                if (!HasPath || DistanceToPathPoint > PRECISION_DISTANCE)
                {
                    return;
                }
                Wait();
                if (IsTimeToGo)
                {
                    GoToNextPoint();
                    TryResetWaitTimer();
                }
            }

            private void Wait()
            {
                _waitTimer += Time.deltaTime;

                if(StateMachine._movementController.IsStopped) return;
                
                StateMachine._movementController.IsStopped = true;
                StateMachine._animationWrapper.PlayIdleSmooth();
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
                    _waitTimer = 0f;
                }
            }
        }
    }
}