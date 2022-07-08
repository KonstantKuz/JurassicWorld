using System;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Component.TargetSearcher;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon
{
    public class LightningStrikeWeapon : BaseWeapon
    {
        [SerializeField] private LightningStrike _lightningStrike;

        private HealthiestEnemySearcher _healthiestEnemySearcher;
        
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private void Awake()
        {
            _healthiestEnemySearcher = gameObject.RequireComponentInParent<HealthiestEnemySearcher>();
        }

        public override void Fire(ITarget target, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            var healthiestUnits = _healthiestEnemySearcher.FindHealthiestUnits(projectileParams.Count);
            foreach (var unit in healthiestUnits)
            {
                CreateLightning().Launch(unit.SelfTarget, projectileParams, hitCallback);
            }
        }

        private Projectile CreateLightning()
        {
            return _worldObjectFactory.CreateObject(_lightningStrike.gameObject).GetComponent<Projectile>();
        }
    }
}
