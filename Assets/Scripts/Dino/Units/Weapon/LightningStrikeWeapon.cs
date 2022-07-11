using System;
using Dino.Extension;
using Dino.Location.Service;
using Dino.Units.Component.TargetSearcher;
using Dino.Units.Target;
using Dino.Units.Weapon.Projectiles;
using Dino.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Dino.Units.Weapon
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
