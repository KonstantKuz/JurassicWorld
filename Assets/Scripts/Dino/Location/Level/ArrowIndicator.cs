using System;
using DG.Tweening;
using UnityEngine;

namespace Dino.Location.Level
{
    public class ArrowIndicator : MonoBehaviour
    {
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _time;
        
        public void PointAt(Transform point, Vector3 offset)
        {
            transform.position = point.position + offset;
            transform.DOJump(point.position + offset, _maxHeight, 1, _time).SetLoops(-1);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
