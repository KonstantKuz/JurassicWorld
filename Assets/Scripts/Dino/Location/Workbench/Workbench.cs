using Dino.Inventory.Service;
using Dino.Units;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Location.Workbench
{
    [RequireComponent(typeof(WorkbenchHudOwner))]
    public class Workbench : MonoBehaviour
    {
        [SerializeField]
        private string _craftReceptId;
        [Inject] 
        private CraftService _craftService;
        
        private WorkbenchHudOwner _workbenchHud;
        public string CraftReceptId => _craftReceptId;

        private void Awake()
        {
            _workbenchHud = GetComponent<WorkbenchHudOwner>();
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (IsPlayer(collider)) {
                _workbenchHud.ShowCraftView(this);
            }
        }
        public void OnTriggerExit(Collider collider)
        {
            if (IsPlayer(collider)) {
                _workbenchHud.Hide();
            }
        }

        private bool IsPlayer(Collider collider)
        {
            var unit = collider.GetComponent<Unit>();
            return unit != null && unit.UnitType == UnitType.PLAYER;
        }

        public bool CanCraft()
        { 
            return _craftService.HasIngredientsForReceipt(_craftReceptId);
        }
        public void Craft()
        {
            if (!CanCraft()) {
                this.Logger().Error($"Workbench, recipe crafting error, missing ingredients, receptId:= {_craftReceptId}");
                return;
            }
            _craftService.Craft(_craftReceptId);
        }
    }
}