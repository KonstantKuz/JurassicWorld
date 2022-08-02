﻿using System;
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
            Camera.transform.position = ClampByLevel(nextPosition);
        }

        private Vector3 ClampByLevel(Vector3 nextPosition)
        {
            if (_world.Level == null)
            {
                return nextPosition;
            }
            
            var levelBounds = CurrentLevel.GetBounds();
            var cameraOffset = nextPosition - transform.position;
            
            nextPosition.x = Mathf.Clamp(nextPosition.x,
                levelBounds.center.x - levelBounds.extents.x + _offsetFromLevelEdge,
                levelBounds.center.x + levelBounds.extents.x - _offsetFromLevelEdge);

            nextPosition.z = Mathf.Clamp(nextPosition.z,
                cameraOffset.z + levelBounds.center.z - levelBounds.extents.z + _offsetFromLevelEdge,
                cameraOffset.z + levelBounds.center.z + levelBounds.extents.z - _offsetFromLevelEdge);

            return nextPosition;
        }
    }
}