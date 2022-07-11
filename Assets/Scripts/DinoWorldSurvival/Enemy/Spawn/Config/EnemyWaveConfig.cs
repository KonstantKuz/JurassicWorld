using System.Runtime.Serialization;

namespace DinoWorldSurvival.Enemy.Spawn.Config
{
    public class EnemyWaveConfig
    {
        [DataMember]
        public int SpawnTime;
        [DataMember]
        public int Count;    
        [DataMember]
        public string EnemyId;       
        [DataMember]
        public int EnemyLevel;
        
    }
}