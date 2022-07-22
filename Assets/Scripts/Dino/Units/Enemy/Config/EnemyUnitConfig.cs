using System.Runtime.Serialization;
using Feofun.Config;

namespace Dino.Units.Enemy.Config
{
    public class EnemyUnitConfig : ICollectionItem<string>
    {
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
        
        public string Id => _id; 
    }
}
