using Dino.Weapon.Config;
using Dino.Weapon.Model;

namespace Dino.Units.Player.Model
{
    public class PlayerWeaponModel : IWeaponModel
    {
        private readonly WeaponConfig _config;

        public PlayerWeaponModel(WeaponConfig config)
        {
            _config = config;
        }

        public float TargetSearchRadius => AttackDistance;

        public float AttackDistance => _config.AttackDistance;

        public float AttackDamage => _config.AttackDamage;
        public float AttackInterval => _config.AttackInterval;
    }
}