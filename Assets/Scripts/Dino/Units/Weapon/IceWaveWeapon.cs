using System;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Weapon.Projectiles;
using Dino.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Dino.Units.Weapon
{
    public class IceWaveWeapon : MonoBehaviour
    {
        [SerializeField] private IceWave _iceWave;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        public void Fire(Transform parent, UnitType targetType, IProjectileParams projectileParams,
            Action<GameObject> hitCallback)
        {
            var wave = CreateWave();
            wave.Launch(targetType, parent, projectileParams, hitCallback);
        }

        private IceWave CreateWave()
        {
            return _worldObjectFactory.CreateObject(_iceWave.gameObject).RequireComponent<IceWave>();
        }
    }
}
