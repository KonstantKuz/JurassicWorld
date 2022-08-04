using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Inventory.Model;
using Dino.Location;
using Dino.Units.Player;
using Dino.Units.Player.Model;
using Dino.Units.Service;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Logger.Extension;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<string, Action<string, BaseWeapon>> _specialWeapons;

        [Inject]
        private StringKeyedConfigCollection<WeaponConfig> _weaponConfigs;
        [Inject]
        private World _world;
        [Inject] 
        private ActiveItemService _activeItemService;
        
        private PlayerUnit Player => _world.GetPlayer();

        public event Action<ItemId, IWeaponModel> OnWeaponFireCallback;
        
        public WeaponService()
        {
            _specialWeapons = new Dictionary<string, Action<string, BaseWeapon>>();
        }
        public void TrySetWeapon(string itemId, BaseWeapon weapon)
        {
            if (IsWeapon(itemId)) {
                Set(itemId, weapon);
            } else {
                this.Logger().Debug($"Inventory item:= {itemId} is not Weapon");
            }
        }
        public void Set(string weaponId, BaseWeapon weapon)
        {
            if (_specialWeapons.ContainsKey(weaponId)) {
                _specialWeapons[weaponId].Invoke(weaponId, weapon);
            } else {
                SetWeapon(weaponId, weapon);
            }
        }
        public void Remove()
        {
            Player.PlayerAttack.DeleteWeapon();
        }
        private bool IsWeapon(string itemId)
        {
            return _weaponConfigs.Contains(itemId);
        }
        private void SetWeapon(string weaponId, BaseWeapon weapon)
        {
            var model = CreateModel(weaponId);
            var attack = Player.PlayerAttack;
            attack.SetWeapon(model, weapon, OnWeaponFire);
        }

        private void OnWeaponFire(IWeaponModel weaponModel)
        {
            OnWeaponFireCallback?.Invoke(_activeItemService.ActiveItemId.Value, weaponModel);
        }

        private PlayerWeaponModel CreateModel(string weaponId)
        {
            var config = _weaponConfigs.Get(weaponId);
            return new PlayerWeaponModel(config);
        }
    }
}