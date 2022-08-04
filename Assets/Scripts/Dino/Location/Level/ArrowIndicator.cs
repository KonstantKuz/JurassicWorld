using System;
using DG.Tweening;
using UnityEngine;

namespace Dino.Location.Level
{
    public class ArrowIndicator : MonoBehaviour
    {
        [SerializeField] private float _maxHeight;
        [SerializeField] private float _time;
        
        public void PointAt(Transform point)
        {
            transform.position = point.position;
            transform.DOJump(point.position, _maxHeight, 1, _time).SetLoops(-1);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
