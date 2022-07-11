using Dino.Location;
using UnityEngine;
using Zenject;

namespace Dino.Squad.Component
{
    public class MovementAnalytics : MonoBehaviour, IWorldScope
    {
        [SerializeField] private float _standingThreshold;

        [Inject] private Joystick _joystick;

        public float StandingTime { get; private set; }

        public void OnWorldSetup()
        {
            StandingTime = 0;
        }

        public void OnWorldCleanUp()
        {
        }

        private void Update()
        {
            if (_joystick.Direction.magnitude >= _standingThreshold) return;
            StandingTime += Time.deltaTime;
        }
    }
}