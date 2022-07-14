﻿using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class IdleState : BaseState
        {
            private IAttackModel _attackModel;
            private Unit Owner => StateMachine._owner;
            private Vector3 TargetPosition => StateMachine.Target.Root.position;
            private float DistanceToTarget => Vector3.Distance(Owner.transform.position, TargetPosition);
            
            public IdleState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
            }            

            public override void OnEnterState()
            {
                StateMachine._agent.isStopped = true;
                StateMachine._animationWrapper.PlayIdleSmooth();
            }

            public override void OnExitState()
            {
            }

            public override void OnTick()
            {
                // test intead of patrol state
                if (DistanceToTarget > _attackModel.AttackDistance * 3)
                {
                    return;
                }
            
                StateMachine.SetState(new ChaseState(StateMachine));
            }
        }
    }
}