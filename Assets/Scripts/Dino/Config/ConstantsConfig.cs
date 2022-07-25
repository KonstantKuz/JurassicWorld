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
        
        [DataMember(Name = "IterationStartLevelIndex")]
        private int _iterationStartLevelIndex;
        
        public string FirstUnit => _firstUnit;
        [CanBeNull]
        public string FirstItem => _firstItem;

        public int IterationStartLevelIndex => _iterationStartLevelIndex;
    }
}