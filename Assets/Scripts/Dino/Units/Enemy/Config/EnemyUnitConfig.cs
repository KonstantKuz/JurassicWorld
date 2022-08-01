using System.Runtime.Serialization;
using Feofun.Config;

namespace Dino.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
        public const int MIN_LEVEL = 1;        

        [DataMember(Name = "Id")] 
        private string _id;
        [DataMember] 
        public int Health;
        [DataMember] 
        public float MoveSpeed;
        [DataMember] 
        public EnemyAttackConfig EnemyAttackConfig;
        [DataMember] 
        public PatrolStateConfig PatrolStateConfig;
        [DataMember]
        public LookAroundStateConfig LookAroundStateConfig;
        [DataMember] 
        public RankParamsConfig RankParamsConfig;
        
        public string Id => _id;

        public int GetHealthForLevel(int level) => Health + (level - MIN_LEVEL) * RankParamsConfig.HealthStep;
        public int GetDamageForLevel(int level) => EnemyAttackConfig.AttackDamage + (level - MIN_LEVEL) * RankParamsConfig.DamageStep;
    }
}
