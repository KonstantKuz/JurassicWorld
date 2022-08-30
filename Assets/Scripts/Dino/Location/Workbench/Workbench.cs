using System;
using Dino.Inventory.Service;
using Dino.Units;
using Logger.Extension;
using UnityEngine;
using Zenject;
using Unit = Dino.Units.Unit;

namespace Dino.Location.Workbench
{
    public class Workbench : MonoBehaviour
    {
        [SerializeField]
        private string _craftRecipeId;
        
        [Inject]
        private CraftService _craftService;

        public event Action OnPlayerTriggered;
        public bool IsPlayerInCraftingArea { get; private set; }
        
        public string CraftRecipeId => _craftRecipeId;

        public bool CanCraftRecipe()
        {
            return _craftService.HasIngredientsForRecipe(_craftRecipeId);
        }
        public void Craft()
        {
            if (!CanCraftRecipe()) {
                this.Logger().Error($"Workbench, recipe crafting error, missing ingredients, receptId:= {_craftRecipeId}");
                return;
            }
            _craftService.Craft(_craftRecipeId);
        }
        private void OnTriggerEnter(Collider collider)
        {
            if (IsPlayer(collider)) {
                OnPlayerTrigger(true);
            }
        }
        private void OnTriggerExit(Collider collider)
        {
            if (IsPlayer(collider)) {
                OnPlayerTrigger(false);
            }
        }
        private void OnPlayerTrigger(bool entered)
        {
            IsPlayerInCraftingArea = entered;
            OnPlayerTriggered?.Invoke();
        }
        private bool IsPlayer(Collider collider)
        {
            var unit = collider.GetComponent<Unit>();
            return unit != null && unit.UnitType == UnitType.PLAYER;
        }

     
    }
}