using System;
using System.Collections.Generic;
using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Location;
using Dino.Units;
using Dino.Units.Player.Attack;
using Dino.Units.Player.Model;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using Feofun.Extension;
using Logger.Extension;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<WeaponId, Action<WeaponId>> Weapons;

        [Inject]
        private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;

        [Inject]
        private World _world;

        private Unit Player => _world.GetPlayer();

        public WeaponService()
        {
            Weapons = new Dictionary<WeaponId, Action<WeaponId>>() {
                    {WeaponId.Stick, CreateMelee}
            };
        }

        public void Set(WeaponId weaponId)
        {
            if (!Weapons.ContainsKey(weaponId)) {
                this.Logger().Error($"Weapon:= {weaponId} - is not registered");
                return;
            }
            var createAction = Weapons[weaponId];
            createAction.Invoke(weaponId);
        }

        public void Set(string inventoryId)
        {
            var weaponId = EnumExt.ValueOf<WeaponId>(inventoryId);
            Set(weaponId);
        }

        public void Remove()
        {
            var attack = Player.GameObject.RequireComponent<PlayerAttack>();
            attack.DeleteWeapon();
        }

        private void CreateMelee(WeaponId weaponId)
        {
            var model = CreateModel(weaponId);
            var inventoryOwner = Player.GameObject.RequireComponent<InventoryOwner>();
            var weapon = inventoryOwner.InventoryItem.RequireComponent<BaseWeapon>();
            var attack = Player.GameObject.RequireComponent<PlayerAttack>();
            attack.SetWeapon(model, weapon);
        }

        private PlayerWeaponModel CreateModel(WeaponId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId);
            return new PlayerWeaponModel(config);
        }
    }
}