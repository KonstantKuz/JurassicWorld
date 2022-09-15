using System.Linq;
using Dino.Extension;
using Dino.Location.Service;
using UnityEngine;

namespace Dino.Location.Level
{
    public class NavigationArrow : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;

        public Transform Parent { get; set; }
        public Transform Target { get; set; }

        private void Update()
        {
            FollowParent();
            PointAtTarget();
        }

        private void FollowParent()
        {
            if(Parent == null) return;
            transform.position = Parent.position;
        }

        private void PointAtTarget()
        {
            if(Target == null) return;
            RotateTo(Target);
        }

        private void RotateTo(Transform target)
        {
            var lookAtDirection = (target.position - transform.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
        }
        
        public static NavigationArrow Spawn(WorldObjectFactory worldObjectFactory)
        {
            var indicatorPrefab = worldObjectFactory.GetPrefabComponents<NavigationArrow>().First();
            var indicator = worldObjectFactory.CreateObject(indicatorPrefab.gameObject).GetComponent<NavigationArrow>();
            return indicator;
        }
    }
}
