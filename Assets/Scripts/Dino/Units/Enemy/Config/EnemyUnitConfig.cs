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
        public float RotationSpeed;
        [DataMember] 
        public AttackVariant AttackVariant;
        [DataMember]
        public float AttackDamage;
        [DataMember]
        public float AttackDistance;
        [DataMember]
        public float AttackInterval;
        [DataMember] 
        public PatrolStateConfig PatrolStateConfig;
        [DataMember]
        public LookAroundStateConfig LookAroundStateConfig;
        [DataMember] 
        public RankParamsConfig RankParamsConfig;
        
        public string Id => _id;

        public int GetHealthForLevel(int level) => Health + (level - MIN_LEVEL) * RankParamsConfig.HealthStep;
        public float GetDamageForLevel(int level, float originDamage) => originDamage + (level - MIN_LEVEL) * RankParamsConfig.DamageStep;
    }
}
