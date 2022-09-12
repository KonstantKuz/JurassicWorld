using System;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Inventory.Service;
using Dino.Location;
using Dino.Session.Messages;
using Dino.Units.Player.Component;
using Dino.Units.Service;
using SuperMaxim.Messaging;
using Zenject;

namespace Dino.Weapon.Service
{
    public class AutomaticWeaponChangeService : IWorldScope
    {
        [Inject]
        private WeaponService _weaponService;
        [Inject]
        private ActiveItemService _activeItemService;
        [Inject]
        private InventoryService _inventoryService;
        [Inject]
        private World _world; 
        [Inject]
        private IMessenger _messenger;
        
        private PlayerAttack PlayerAttack => _world.RequirePlayer().PlayerAttack;

        public void OnWorldSetup()
        {
            _messenger.Subscribe<SessionStartMessage>(OnSessionStart);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStart);
            _messenger.Unsubscribe<SessionEndMessage>(OnSessionFinished);
        }
        private void OnSessionFinished(SessionEndMessage obj)
        {
            PlayerAttack.OnAttacked -= OnPlayerAttacked;
        }
        private void OnSessionStart(SessionStartMessage obj)
        {
            PlayerAttack.OnAttacked += OnPlayerAttacked;
        }
        private void OnPlayerAttacked()
        {
            if (PlayerAttack.WeaponWrapper == null) {
                throw new NullReferenceException("WeaponWrapper is null on active player atack");
            }
            if (!PlayerAttack.WeaponWrapper.Clip.HasAmmo) {
                TryChangeWeapon();
            }
        }
        private void TryChangeWeapon()
        {
            var newWeapon = _inventoryService.GetItems(InventoryItemType.Weapon)
                                             .Select(it => _weaponService.GetWeaponWrapper(it.Id))
                                             .FirstOrDefault(it => it.Clip.HasAmmo);
            if (newWeapon == null) {
                return;
            }
            _activeItemService.Replace(_inventoryService.GetItem(newWeapon.WeaponId));
        }
    }
}