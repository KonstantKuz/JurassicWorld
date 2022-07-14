using Dino.Extension;
using Dino.Location;
using Dino.Units.Enemy.Model;
using Dino.Units.Model;
using Dino.Units.Weapon;
using Dino.Units.Weapon.Projectiles;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.Units.StateMachine
{
    public partial class EnemyStateMachine
    {
        private class PatrolState : BaseState
        {
            private const float PRECISION_DISTANCE = 0.2f;

            private PatrolPath _patrolPath;
            private EnemyBehaviourModel _behaviourModel;
            
            private float _waitTimer;

            private Unit Owner => StateMachine._owner;
            private Vector3 TargetPosition => StateMachine.Target.Root.position;
            private Transform NextPathPoint { get; set; }
            private float DistanceToPathPoint => Vector3.Distance(Owner.transform.position, NextPathPoint.position);
            
            public PatrolState(EnemyStateMachine stateMachine) : base(stateMachine)
            {
                _patrolPath = stateMachine._patrolPath;
                _behaviourModel = stateMachine._behaviourModel;
                var coneRenderer = StateMachine._owner.GetComponentInChildren<RangeConeRenderer>();
                coneRenderer.Build(_behaviourModel.FieldOfViewAngle / 2, _behaviourModel.FieldOfViewDistance);
            }
            
            public override void OnEnterState()
            {
                StateMachine._agent.isStopped = true;
                StateMachine._animationWrapper.PlayIdleSmooth();

                GotoNextPoint();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                SearchPlayer();
                if (DistanceToPathPoint > PRECISION_DISTANCE)
                {
                    return;
                }
                Wait();
            }

            private void SearchPlayer()
            {
                var isPlayerInView =
                    IsInsideCone(TargetPosition, Owner.transform.position,
                        Owner.transform.forward, _behaviourModel.FieldOfViewAngle / 2) && 
                    IsInsideDistanceRange(TargetPosition, Owner.transform.position, 0, _behaviourModel.FieldOfViewDistance);
                
                if (isPlayerInView)
                {
                    StateMachine.SetState(new ChaseState(StateMachine));
                }
            }

            private void Wait()
            {
                StateMachine._agent.isStopped = true;
                
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= _behaviourModel.PatrolIdleTime)
                {
                    GotoNextPoint();
                }
            }

            private void GotoNextPoint()
            {
                NextPathPoint = _patrolPath.Pop();
                StateMachine._agent.SetDestination(NextPathPoint.position);
                StateMachine._agent.isStopped = false;

                if (_patrolPath.WaitOnEveryPoint || _patrolPath.IsEndOfPath(NextPathPoint))
                {
                    _waitTimer = 0f;
                }
            }

            private static bool IsInsideCone(Vector3 target, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle)
            {
                var targetDirection = target - coneOrigin;
                var angle = Vector3.Angle(coneDirection, targetDirection.XZ());
                return angle <= maxAngle;
            }

            private static bool IsInsideDistanceRange(Vector3 target, Vector3 origin, float distanceMin, float distanceMax)
            {
                var distance = Vector3.Distance(origin, target);
                return distance > distanceMin && distance < distanceMax;
            }
        }
    }

}