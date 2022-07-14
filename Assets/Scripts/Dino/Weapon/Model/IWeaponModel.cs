using Dino.Weapon.Config;

namespace Dino.Weapon.Model
{
    public class IWeaponModel : Units.Model.IWeaponModel
    {
        private readonly WeaponConfig _config;

        public IWeaponModel(WeaponConfig config)
        {
            _config = config;
        }

        public float TargetSearchRadius => AttackDistance;

        public float AttackDistance => _config.AttackDistance;

        public float AttackDamage => _config.AttackDamage;
        public float AttackInterval => _config.AttackInterval;
    }
}