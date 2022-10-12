using Dino.Units.Enemy.Model;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class LookAroundState : BaseState
        {
            private const float LOOK_ANGLE_PRECISION = 1f;
            
            private readonly Vector3 _desiredPosition;

            private Unit Owner => StateMachine._owner;

            public LookAroundState(UnitStateMachine stateMachine, Vector3? desiredPosition = null) : base(stateMachine)
            {
                _desiredPosition = desiredPosition ?? Owner.transform.position - Owner.transform.forward;
            }

            public override void OnEnterState()
            {
                StateMachine.Stop();
                Owner.Damageable.OnDamageTaken += StateMachine.LookTowardsDamage;
            }

            public override void OnExitState()
            {
                Owner.Damageable.OnDamageTaken -= StateMachine.LookTowardsDamage;
            }
            
            public override void OnTick()
            {
                if (StateMachine.ChaseTargetIfExists())
                {
                    return;
                }

                if (GetAngleToDesiredPosition() > LOOK_ANGLE_PRECISION)
                {
                    StateMachine._movementController.RotateTo(_desiredPosition);
                    return;
                }
                
                StateMachine.SetState(UnitState.GoToPoint, _desiredPosition);
            }

            private float GetAngleToDesiredPosition()
            {
                var directionToDesiredPos = _desiredPosition - Owner.transform.position;
                return Vector3.Angle(directionToDesiredPos, Owner.transform.forward);
            }
        }
    }
}