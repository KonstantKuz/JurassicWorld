using System;
using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Location.Service;
using DinoWorldSurvival.Units.Component.TargetSearcher;
using DinoWorldSurvival.Units.Target;
using DinoWorldSurvival.Units.Weapon.Projectiles;
using DinoWorldSurvival.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.Units.Weapon
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
