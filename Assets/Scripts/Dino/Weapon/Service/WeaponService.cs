using System;
using System.Collections.Generic;
using Dino.Location;
using Dino.Units;
using Dino.Weapon.Model;
using Zenject;

namespace Dino.Weapon.Service
{
    public class WeaponService
    {
        private readonly Dictionary<WeaponId, Action> Weapons;
        
        [Inject]
        private World _world;
        
        
        private Unit Player => _world.GetPlayer();
        
        public WeaponService()
        {
            Weapons = new Dictionary<WeaponId, Action>() {
                    {WeaponId.Stick, CreateMeleeWeapon}
            };
        }

        public void Add(WeaponId weaponId)
        {
            
        } 
        public void Remove()
        {
            
        }

        private void CreateMeleeWeapon()
        {
            
        }
    }
}