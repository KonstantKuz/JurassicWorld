using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Dino.Location.Workbench
{
    public class Workbench : MonoBehaviour
    {
        [SerializeField] private string _craftItemId;
        [SerializeField] private float _craftDuration;

        [Inject] private DiContainer _diContainer;
        
        [CanBeNull]
        private CrafterByTimer _crafterByTimer;
        public event Action<CrafterByTimer> OnCrafterCreated;
        public event Action OnCrafterRemoved;
        
        public string CraftItemId => _craftItemId;

        private void OnTriggerEnter(Collider other)
        {
            _crafterByTimer = CreateCrafter();
            OnCrafterCreated?.Invoke(_crafterByTimer);
        }

        private void OnTriggerStay(Collider collider)
        {
            _crafterByTimer?.Update();
        }

        private void OnTriggerExit(Collider collider)
        {
            _crafterByTimer?.Dispose();
            _crafterByTimer = null;
            OnCrafterRemoved?.Invoke();
        }
        private CrafterByTimer CreateCrafter()
        {
            return _diContainer.Instantiate<CrafterByTimer>(new[] {(object) _craftItemId, _craftDuration});
        }
    }
}