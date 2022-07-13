﻿namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class DeathState : BaseState
        {
            public DeathState(StateMachine.UnitStateMachine stateMachine) : base(stateMachine)
            {
            }

            public override void OnEnterState()
            {
                StateMachine._agent.isStopped = true;
            }

            public override void OnTick()
            {
            }

            public override void OnExitState()
            {
            }
        }
    }
}