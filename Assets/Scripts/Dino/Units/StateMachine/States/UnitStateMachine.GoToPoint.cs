using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class GoToPointState : BaseState
        {
            private const float DESTINATION_REACHED_DISTANCE = 0.1f;
            
            private Vector3 _destination;
            
            public GoToPointState(UnitStateMachine stateMachine, Vector3 destination) : base(stateMachine)
            {
                _destination = destination;
            }

            public override void OnEnterState()
            {
                StateMachine.GoToPoint(_destination);
            }

            public override void OnTick()
            {
                if (StateMachine.ChaseTargetIfExists())
                {
                    return;
                }

                if (Vector3.Distance(_destination, StateMachine.transform.position) > DESTINATION_REACHED_DISTANCE)
                {
                    return;
                }

                StateMachine.SetState(UnitState.Patrol);
            }

            public override void OnExitState()
            {
            }
        }
    }
}