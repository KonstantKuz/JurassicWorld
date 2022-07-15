using System.Runtime.Serialization;

namespace Dino.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;      
        
        [DataMember(Name = "FirstItem")]
        private string _firstItem;
        
        public string FirstUnit => _firstUnit;       
        public string FirstItem => _firstItem;
    }
}