using System;
using DG.Tweening;
using Dino.Location;
using Dino.Location.Level;
using UnityEngine;
using Zenject;

namespace Dino.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] 
        private float _distance;
        [SerializeField]
        private float _offsetFromLevelEdge;
        
        [Inject] private World _world;
        
        private Level CurrentLevel => _world.Level;
        public Transform Target { get; set; }
        public bool IsFollowTarget { get; set; } = true;

        private void Update()
        {
            FollowTarget();
        }

        private void FollowTarget()
        {
            if(Target == null || !IsFollowTarget) return;
            
            var nextPosition = Target.position - _distance * transform.forward;
            transform.position = ClampByLevel(nextPosition);
        }

        private Vector3 ClampByLevel(Vector3 position)
        {
            if (_world.Level == null)
            {
                return position;
            }
            
            var levelBounds = CurrentLevel.GetBounds();
            var cameraOffset = position - Target.position;
            
            position.x = Mathf.Clamp(position.x,
                levelBounds.center.x - levelBounds.extents.x + _offsetFromLevelEdge,
                levelBounds.center.x + levelBounds.extents.x - _offsetFromLevelEdge);

            position.z = Mathf.Clamp(position.z,
                cameraOffset.z + levelBounds.center.z - levelBounds.extents.z + _offsetFromLevelEdge,
                cameraOffset.z + levelBounds.center.z + levelBounds.extents.z - _offsetFromLevelEdge);

            return position;
        }

        public void PlayLookAt(Vector3 point, float speed, float time, Action onComplete)
        {
            IsFollowTarget = false;
            
            var initialPosition = transform.position;
            var finalPosition = point - _distance * transform.forward;
            var distanceToPoint = Vector3.Distance(initialPosition, finalPosition);
            var moveTime = distanceToPoint / speed;
            
            var moveToPoint = transform.DOMove(finalPosition, moveTime);
            var moveBack = transform.DOMove(initialPosition, moveTime);
            var lookAt = DOTween.Sequence().Append(moveToPoint).AppendInterval(time).Append(moveBack).SetEase(Ease.Linear);
          
            lookAt.onComplete = () =>
            {
                onComplete?.Invoke();
                IsFollowTarget = true;
            };
        }
    }
}