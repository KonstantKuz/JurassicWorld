using Dino.Units.Model;
using Dino.Weapon.Config;

namespace Dino.Weapon.Model
{
    public class WeaponModel : IWeapon
    {
        private readonly WeaponConfig _config;

        public WeaponModel(WeaponConfig config)
        {
            _config = config;
        }

        public float TargetSearchRadius => AttackDistance;

        public float AttackDistance => _config.AttackDistance;

        public float AttackDamage => _config.AttackDamage;
        public float AttackInterval => _config.AttackInterval;
    }
}