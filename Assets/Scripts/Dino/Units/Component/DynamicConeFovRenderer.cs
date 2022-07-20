using Feofun.Components;
using UnityEngine;

namespace Dino.Units.Component
{
    public class DynamicConeFovRenderer : StaticConeFovRenderer, IUpdatableComponent
    {
        [SerializeField] private float _updatePeriod;
        [SerializeField] private LayerMask _mask;

        private float _timer;
        private RaycastHit[] _hits;

        protected override void Awake()
        {
            base.Awake();
            _hits = new RaycastHit[1];
        }

        public void OnTick()
        {
            if (_segmentsCount == 0) return;

            _timer += Time.deltaTime;
            if (_timer < _updatePeriod) return;
            _timer = 0f;
            if (HasObstaclesInRadius())
            {
                _mesh.vertices = BuildVertices(_segmentsCount, _angle, _radius);
            }
        }

        private bool HasObstaclesInRadius()
        {
            return Physics.CheckSphere(transform.position, _radius, _mask.value);
        }

        protected override Vector3 GetVertPositionAtAngle(float angle, float radius)
        {
            var hasHit = HasHit(angle, radius);

            var distance = hasHit != 0 ? _hits[0].distance : radius;
            var x = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
            var z = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;
            return new Vector3(x, 0, z);
        }
        
        private int HasHit(float angle, float radius)
        {
            return Physics.RaycastNonAlloc(transform.position, Quaternion.Euler(0, angle, 0) * transform.forward,
                _hits, radius, _mask.value);
        }
    }
}