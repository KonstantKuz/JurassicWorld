using System.Runtime.Serialization;
using Dino.Loot.Config;
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
        [DataMember] 
        public DroppingLootConfig DroppingLootConfig;      
        
        public string Id => _id; 
    }
}
