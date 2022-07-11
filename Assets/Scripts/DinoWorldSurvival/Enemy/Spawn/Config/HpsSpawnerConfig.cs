using System.Runtime.Serialization;

namespace DinoWorldSurvival.Enemy.Spawn.Config
{
    public class HpsSpawnerConfig
    {
        [DataMember]
        public float StartingHPS;
        [DataMember]
        public float HPSSpeed;
        [DataMember]
        public int MinWaveSize;
        [DataMember]
        public int MaxWaveSize;
        [DataMember]
        public float MinInterval;
        [DataMember]
        public float MaxInterval;
        [DataMember] 
        public string EnemyId;
    }
}