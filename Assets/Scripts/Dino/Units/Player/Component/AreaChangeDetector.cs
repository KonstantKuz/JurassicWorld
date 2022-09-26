using Logger.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Player.Component
{
    public class AreaChangeDetector : MonoBehaviour
    {
        private int _lastAreaMask = 0;
        
        private void Update()
        {
            if (!NavMesh.SamplePosition(transform.position, out var hit, 0.1f, NavMesh.AllAreas))
            {
                return;
            }
            if (hit.mask == _lastAreaMask) return;
            
            this.Logger().Debug($"Area changed: {hit.mask}");
            _lastAreaMask = hit.mask;
        }
    }
}