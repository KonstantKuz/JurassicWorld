using System;
using Dino.Units.Enemy.Config;
using AppContext = Feofun.App.AppContext;

namespace Dino.Units.Enemy.Model.EnemyAttack
{
    public class EnemyAttackModel
    {
        public EnemyAttackModel(int level, EnemyUnitConfig unitConfig)
        {
            AttackVariant = unitConfig.AttackVariant;
            var damage = unitConfig.GetDamageForLevel(level, unitConfig.AttackDamage);
            AttackDamage = damage;
            AttackDistance = unitConfig.AttackDistance;
            AttackInterval = unitConfig.AttackInterval;
            AttackConfigs = AppContext.Container.Resolve<EnemyAttackConfigs>();
        }
        
        public AttackVariant AttackVariant { get; }
        public float AttackDamage { get; }
        public float AttackDistance { get; }
        public float AttackInterval { get; }
        private EnemyAttackConfigs AttackConfigs { get; }

        public T CreateCurrentVariantModel<T>() where T : AttackVariantModel
        {
            return CreateVariantModel<T>(AttackVariant);
        }

        public T CreateVariantModel<T>(AttackVariant attackVariant) where T : AttackVariantModel
        {
            return attackVariant switch
            {
                AttackVariant.Bulldozing => new BulldozingAttackModel(AttackConfigs.Get<BulldozingAttackConfig>(attackVariant)) as T,
                AttackVariant.Jumping => new JumpingAttackModel(AttackConfigs.Get<JumpingAttackConfig>(attackVariant)) as T,
                _ => throw new ArgumentOutOfRangeException(nameof(attackVariant), attackVariant, null)
            };
        }
    }
}