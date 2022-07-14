using Dino.Extension;
using Dino.Location;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Enemy.Model;
using Dino.Units.StateMachine.States;
using Dino.Units.Target;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class PatrolState : BaseState
        {
            private const float PRECISION_DISTANCE = 0.2f;

            private readonly PatrolPath _patrolPath;
            private readonly PatrolStateModel _stateModel;
            private readonly ITargetProvider _targetProvider;
            
            private float _waitTimer;
            private PatrolStateHelper _stateHelper;

            private Unit Owner => StateMachine._owner;
            private PatrolStateHelper StateHelper => _stateHelper ??= Owner.gameObject.RequireComponent<PatrolStateHelper>();
            private Transform NextPathPoint { get; set; }
            private float DistanceToPathPoint => Vector3.Distance(Owner.transform.position, NextPathPoint.position);
            private bool IsTimeToGo => _waitTimer >= _stateModel.PatrolIdleTime;

            public PatrolState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = (EnemyUnitModel) Owner.Model;
                Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel.");
                _stateModel = enemyModel.PatrolStateModel;
                _patrolPath = StateHelper.PatrolPath;
                _targetProvider = Owner.gameObject.RequireComponent<ITargetProvider>();
            }
            
            public override void OnEnterState()
            {
                GoToNextPoint();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                if (_targetProvider.Target != null)
                {
                    StateMachine.SetState(UnitState.Chase);
                }
                if (DistanceToPathPoint > PRECISION_DISTANCE)
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
                NextPathPoint = _patrolPath.Pop();
                StateMachine._movementController.MoveTo(NextPathPoint.position);
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
            }

            private void TryResetWaitTimer()
            {
                if (_patrolPath.WaitOnEveryPoint || _patrolPath.IsEndOfPath(NextPathPoint))
                {
                    _waitTimer = 0f;
                }
            }
        }
    }
}