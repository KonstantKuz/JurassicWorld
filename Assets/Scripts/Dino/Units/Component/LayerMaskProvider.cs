using UnityEngine;

namespace Dino.Units.Component
{
    public class LayerMaskProvider : MonoBehaviour
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private LayerMask _damageMask;

        public LayerMask ObstacleMask => _obstacleMask;
        public LayerMask DamageMask => _damageMask;
    }
}
