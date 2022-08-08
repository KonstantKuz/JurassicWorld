using System;
using System.Linq;
using DG.Tweening;
using Dino.Location.Service;
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
        
        public static ArrowIndicator SpawnAbove(WorldObjectFactory worldObjectFactory, Transform point, Vector3 offset)
        {
            var indicatorPrefab = worldObjectFactory.GetPrefabComponents<ArrowIndicator>().First();
            var indicator = worldObjectFactory.CreateObject(indicatorPrefab.gameObject).GetComponent<ArrowIndicator>();
            indicator.PointAt(point, offset);
            return indicator;
        }
    }
}
