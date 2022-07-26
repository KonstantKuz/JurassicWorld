using System.Collections;
using System.Collections.Generic;
using Dino.Extension;
using Dino.Units.Enemy.Model;
using Dino.Units.StateMachine.States;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class LookAroundState : BaseState
        {
            private const float LOOK_ANGLE_PRECISION = 1f;
            
            private readonly LookAroundStateModel _stateModel;
            private readonly Vector3 _desiredPosition;

            private Unit Owner => StateMachine._owner;

            public LookAroundState(UnitStateMachine stateMachine, Vector3? desiredPosition = null) : base(stateMachine)
            {
                _stateModel = Owner.RequireEnemyModel().LookAroundStateModel;
                _desiredPosition = desiredPosition ?? Owner.transform.position - Owner.transform.forward;
            }

            public override void OnEnterState()
            {
                StateMachine._waitTimer = 0f;
                Owner.Damageable.OnDamageTaken += StateMachine.LookAround;
            }

            public override void OnExitState()
            {
                Owner.Damageable.OnDamageTaken -= StateMachine.LookAround;
            }

            public override void OnTick()
            {
                StateMachine.ChaseTargetIfExists();
                StateMachine.Wait(_stateModel.LookAroundTime, OnWaitTimeEnd);

                if (GetAngleToDesiredPosition() > LOOK_ANGLE_PRECISION)
                {
                    StateMachine._movementController.RotateTo(_desiredPosition, _stateModel.LookAroundSpeed);
                }
            }

            private void OnWaitTimeEnd()
            {
                StateMachine._waitTimer = 0f;
                StateMachine.SetState(UnitState.Patrol);
            }

            private float GetAngleToDesiredPosition()
            {
                var directionToDesiredPos = Owner.transform.position - _desiredPosition;
                return Vector3.Angle(directionToDesiredPos, Owner.transform.forward);
            }
        }
    }
}