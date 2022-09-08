using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Location.Service;
using Dino.Units.Player;
using Dino.Weapon.Service;
using Logger.Extension;
using UniRx;
using Zenject;

namespace Dino.Units.Service
{
    public class ActiveItemService
    {
        private readonly ReactiveProperty<ItemId> _activeItemId = new ReactiveProperty<ItemId>(null);
        private readonly InventoryService _inventoryService;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private World _world;
        [Inject] private WeaponService _weaponService;

        private ActiveItemRepository _repository;

        public IReadOnlyReactiveProperty<ItemId> ActiveItemId => _activeItemId;
        private PlayerUnit Player => _world.RequirePlayer();

        public ActiveItemService(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            inventoryService.OnItemAdded += OnItemAdded;
            inventoryService.OnItemRemoved += OnItemRemoved;
        }

        public void Init()
        {
            _repository = new ActiveItemRepository();

            if (!_repository.Exists()) {
                _repository.Set(null);
            } else {
                Equip(_repository.Get());
            }
        }

        public void Save()
        {
            _repository.Set(ActiveItemId.Value);
        }

        public bool HasActiveItem() => _activeItemId.HasValue && _activeItemId.Value != null;
        public bool IsActiveItem(ItemId itemId) => HasActiveItem() && itemId.Equals(_activeItemId.Value);

        public void Replace(ItemId itemId)
        {
            UnEquip();
            Equip(itemId);
        }

        public void Equip(ItemId itemId)
        {
            if (!_inventoryService.Contains(itemId)) {
                this.Logger().Error($"Equip error, inventory must contain the item:= {itemId}");
                return;
            }
            if (!IsItemTypeEquipable(itemId)) {
                this.Logger().Error($"Equip error, type of inventory item must be equipable, item:= {itemId}");
                return;
            }
            if (_activeItemId.Value != null) {
                RemoveActiveItemObject();
            }
            var itemOwner = Player.ActiveItemOwner;
            var itemObject = _worldObjectFactory.CreateObject(itemId.Name, itemOwner.Container);
            itemOwner.Set(itemObject);

            _weaponService.SetActiveWeapon(itemId, itemOwner.GetWeapon());
            _activeItemId.SetValueAndForceNotify(itemId);
        }

        public void UnEquip()
        {
            RemoveActiveItemObject();
            _activeItemId.SetValueAndForceNotify(null);
        }

        public void RemoveActiveItemObject()
        {
            _weaponService.RemoveActiveWeapon();
            Player.ActiveItemOwner.Remove();
        }

        private void OnItemAdded(ItemId itemId) => TryEquipAddedItem(itemId);

        private void OnItemRemoved(ItemId itemId)
        {
            if (IsActiveItem(itemId)) {
                UnEquip();
            }
        }

        private bool IsItemTypeEquipable(ItemId itemId) => itemId.Type.IsEquipable();

        private void TryEquipAddedItem(ItemId itemId)
        {
            if (!IsItemTypeEquipable(itemId)) {
                return;
            }
            if (!HasActiveItem() || itemId.Rank >= ActiveItemId.Value.Rank) {
                Replace(itemId);
            }
        }
    }
}