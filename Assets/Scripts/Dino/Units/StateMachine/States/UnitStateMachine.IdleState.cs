using Dino.Units.Model;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class IdleState : BaseState
        {
            public IdleState(UnitStateMachine stateMachine) : base(stateMachine)
            {
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
            }
        }
    }
}