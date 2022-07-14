using System.Runtime.Serialization;

namespace Dino.Core.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;
        
        public string FirstUnit => _firstUnit;
    }
}