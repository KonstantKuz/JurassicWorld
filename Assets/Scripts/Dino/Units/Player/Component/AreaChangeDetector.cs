using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Player.Component
{
    public class AreaChangeDetector : MonoBehaviour
    {
        private const int GRASS_AREA_MASK = 8;
        
        public enum AreaType
        {
            Default,
            Grass
        };

        private int _lastAreaMask = 0;
        private readonly ReactiveProperty<AreaType> _currentAreaType = new ReactiveProperty<AreaType>();

        public IReadOnlyReactiveProperty<AreaType> CurrentAreaType => _currentAreaType;

        private void Update()
        {
            if (!NavMesh.SamplePosition(transform.position, out var hit, 0.1f, NavMesh.AllAreas))
            {
                return;
            }
            if (hit.mask == _lastAreaMask) return;
            _lastAreaMask = hit.mask;
            _currentAreaType.Value = GetAreaType(_lastAreaMask);            
        }

        private static AreaType GetAreaType(int areaMask)
        {
            return areaMask switch
            {
                GRASS_AREA_MASK => AreaType.Grass,
                _ => AreaType.Default
            };
        }
    }
}