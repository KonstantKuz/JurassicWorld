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
            private readonly WaitSubState _waitSubState;

            private Unit Owner => StateMachine._owner;

            public LookAroundState(UnitStateMachine stateMachine, Vector3? desiredPosition = null) : base(stateMachine)
            {
                _stateModel = Owner.RequireEnemyModel().LookAroundStateModel;
                _desiredPosition = desiredPosition ?? Owner.transform.position - Owner.transform.forward;
                _waitSubState = WaitSubState.Build(_stateModel.LookAroundTime, StateMachine.Stop, null, () => StateMachine.SetState(UnitState.Patrol));
            }

            public override void OnEnterState()
            {
                StateMachine.Stop();
                Owner.Damageable.OnDamageTaken += StateMachine.LookAround;
            }

            public override void OnExitState()
            {
                Owner.Damageable.OnDamageTaken -= StateMachine.LookAround;
            }
            
            public override void OnTick()
            {
                if (StateMachine.ChaseTargetIfExists())
                {
                    return;
                }

                if (GetAngleToDesiredPosition() > LOOK_ANGLE_PRECISION)
                {
                    StateMachine._movementController.RotateTo(_desiredPosition, _stateModel.LookAroundSpeed);
                    return;
                }
                
                _waitSubState.OnTick();
            }

            private float GetAngleToDesiredPosition()
            {
                var directionToDesiredPos = _desiredPosition - Owner.transform.position;
                return Vector3.Angle(directionToDesiredPos, Owner.transform.forward);
            }
        }
    }
}