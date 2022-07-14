using System.Runtime.Serialization;

namespace Dino.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;      
        
        [DataMember(Name = "FirstInventory")]
        private string _firstInventory;
        
        public string FirstUnit => _firstUnit;       
        public string FirstInventory => _firstInventory;
    }
}