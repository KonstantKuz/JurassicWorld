using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Dino.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;      
        
        [DataMember(Name = "FirstItem")]
        private string _firstItem;    
        
        [DataMember(Name = "LoopStartLevelIndex")]
        private int _loopStartLevelIndex;
        
        public string FirstUnit => _firstUnit;
        [CanBeNull]
        public string FirstItem => _firstItem;

        public int LoopStartLevelIndex => _loopStartLevelIndex;
    }
}