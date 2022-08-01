using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location;
using Dino.Location.Level;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Dino.Camera
{
    public class CameraFollowBehavior : MonoBehaviour
    {
        [SerializeField] 
        private float _distance;
        [SerializeField]
        private float _offsetFromLevelEdge;

        [Inject] private World _world;
        
        private UnityEngine.Camera Camera => UnityEngine.Camera.main;
        private Level CurrentLevel => _world.Level;

        private void Update()
        {
            var nextPosition = transform.position - _distance * Camera.transform.forward;
            if (_world.Level == null)
            {
                Camera.transform.position = nextPosition;
                return;
            }
            
            var levelBounds = new Bounds();
            foreach (var bounds in CurrentLevel.GroundBounds) 
            {
                levelBounds.Encapsulate(bounds);
            }
            
            nextPosition.x = Mathf.Clamp(nextPosition.x,
                levelBounds.center.x - levelBounds.extents.x + _offsetFromLevelEdge,
                levelBounds.center.x + levelBounds.extents.x - _offsetFromLevelEdge);

            var cameraOffset = nextPosition - transform.position;
            
            nextPosition.z = Mathf.Clamp(nextPosition.z,
                levelBounds.center.z + cameraOffset.z - levelBounds.extents.z + _offsetFromLevelEdge,
                levelBounds.center.z + cameraOffset.z + levelBounds.extents.z - _offsetFromLevelEdge);
            
            Camera.transform.position = nextPosition;
        }
    }
}