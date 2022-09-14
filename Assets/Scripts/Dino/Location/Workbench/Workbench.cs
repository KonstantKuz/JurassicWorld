using System;
using UnityEngine;

namespace Dino.Location.Workbench
{
    public class Workbench : MonoBehaviour
    {
        [SerializeField]
        private string _craftItemId;

        public event Action OnPlayerTriggered;
        public bool IsPlayerInCraftingArea { get; private set; }

        public string CraftItemId => _craftItemId;

        
        private void OnTriggerEnter(Collider collider)
        {
            OnPlayerTrigger(true);
        }

        private void OnTriggerExit(Collider collider)
        {   
            OnPlayerTrigger(false);
        }


        private void OnPlayerTrigger(bool entered)
        {
            IsPlayerInCraftingArea = entered;
            OnPlayerTriggered?.Invoke();
        }
    }
}