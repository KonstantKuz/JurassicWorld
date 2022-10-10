using Dino.Units.Component.Health;
using Feofun.Extension;
using UnityEngine;

namespace Dino.Units.Component
{
    [RequireComponent(typeof(IDamageable))]
    public class ChangeConeColorByAwareness: MonoBehaviour
    {
        [SerializeField] private Material _unawareMaterial;
        [SerializeField] private Material _awareMaterial;

        private bool _wasUnAwareLastFrame;
        private IDamageable _damageable;
        private StaticConeFovRenderer _coneFovRenderer;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
            _coneFovRenderer = gameObject.RequireComponentInChildren<StaticConeFovRenderer>();
            _wasUnAwareLastFrame = _damageable.IsUnAware;
        }

        private void Update()
        {
            if (_damageable.IsUnAware == _wasUnAwareLastFrame) return;
            _wasUnAwareLastFrame = _damageable.IsUnAware;
            _coneFovRenderer.Material = _wasUnAwareLastFrame ? _unawareMaterial : _awareMaterial;
        }
    }
}