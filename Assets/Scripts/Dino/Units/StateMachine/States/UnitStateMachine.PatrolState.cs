using Dino.Units.StateMachine.States;
using Feofun.Extension;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class PatrolState : BaseState
        {
            private const float PRECISION_DISTANCE = 0.2f;

            private readonly WaitSubState _waitSubState;

            private PatrolPathProvider _pathProvider;
            private PatrolPathIterator _pathIterator;

            private Unit Owner => StateMachine._owner;
            private PatrolPathProvider PathProvider => _pathProvider ??= Owner.gameObject.RequireComponent<PatrolPathProvider>();
            private PatrolPathIterator PathIterator => _pathIterator ??= Owner.gameObject.RequireComponent<PatrolPathIterator>();

            private bool HasPath => PathProvider.PatrolPath != null;
            private Transform CurrentPathPoint => PathProvider.PatrolPath.Path[PathIterator.CurrentPointIndex];
            private float DistanceToPoint => Vector3.Distance(Owner.transform.position, CurrentPathPoint.position);
            
            public PatrolState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var stateModel = Owner.RequireEnemyModel().PatrolStateModel;
                _waitSubState = WaitSubState.Build(stateModel.PatrolIdleTime, StateMachine.Stop, null, SetNextPointAndGo);
            }

            public override void OnEnterState()
            {
                if (!HasPath)
                {
                    StateMachine.SetPatrolPath(null);
                    StateMachine.SwitchToIdle();
                    return;
                }
                
                StateMachine.SetPatrolPath(PathProvider.PatrolPath);
                GoToCurrentPoint();
                Owner.Damageable.OnDamageTaken += StateMachine.LookTowardsDamage;
            }

            public override void OnExitState()
            {
                if (!HasPath) return;
                
                Owner.Damageable.OnDamageTaken -= StateMachine.LookTowardsDamage;
            }

            private void SetNextPointAndGo()
            {
                PathIterator.IncreaseCurrentIndex(PathProvider.PatrolPath.Path.Length);
                GoToCurrentPoint();
            }

            public override void OnTick()
            {
                if (StateMachine.ChaseTargetIfExists())
                {
                    return;
                }
                
                if (!HasPath || DistanceToPoint > PRECISION_DISTANCE)
                {
                    return;
                }
                
                _waitSubState.OnTick();
            }

            private void GoToCurrentPoint()
            {
                StateMachine.GoToPoint(CurrentPathPoint.position);
            }
        }
    }
}