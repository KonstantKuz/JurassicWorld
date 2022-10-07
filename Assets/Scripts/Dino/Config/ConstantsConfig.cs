using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Dino.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;

        [DataMember(Name = "ItemRespawnTime")] 
        private float _itemRespawnTime;
        
        [DataMember(Name = "LoopStartLevelIndex")]
        private int _loopStartLevelIndex;

        [DataMember(Name = "CriticalDamageMultiplier")]
        private float _criticalDamageMultiplier;
        
        public string FirstUnit => _firstUnit;
        public float ItemRespawnTime => _itemRespawnTime;
        public int LoopStartLevelIndex => _loopStartLevelIndex;

        public float CriticalDamageMultiplier => _criticalDamageMultiplier;
        public bool IsCriticalDamageEnabled => CriticalDamageMultiplier >= 1f;
    }
}