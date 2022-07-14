using System;
using System.Collections.Generic;
using Dino.Extension;
using Dino.Location;
using Dino.Units;
using Dino.Units.Player.Attack;
using Dino.Units.Player.Model;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Config;
using UnityEngine;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<WeaponId, Action> Weapons;
        
        [Inject] private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;
        
        [Inject]
        private World _world;

        private Unit Player => _world.GetPlayer();
        
        public WeaponService()
        {
            Weapons = new Dictionary<WeaponId, Action>() {
                    {WeaponId.Stick, CreateMeleeWeapon}
            };
        }

        public void Set(WeaponId weaponId, GameObject inventoryItem)
        {
            
        } 
        public void Remove()
        {
            var attack = Player.GameObject.RequireComponent<PlayerAttack>();
            attack.DeleteWeapon();
        }

        private void CreateMeleeWeapon(WeaponId weaponId, GameObject inventoryItem)
        {
            var model = CreateModel(weaponId);
            var baseWeapon = inventoryItem.GetComponent<BaseWeapon>();
            var attack = Player.GameObject.RequireComponent<PlayerAttack>();
            attack.SetWeapon();

        }

        private PlayerWeaponModel CreateModel(WeaponId weaponId)
        {
            var config = _weaponConfigs.Get(weaponId); 
            return new PlayerWeaponModel(config);
        }
    }
}