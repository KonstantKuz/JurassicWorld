using System.Runtime.Serialization;
using DinoWorldSurvival.Player.Wallet;

namespace DinoWorldSurvival.Shop.Config
{
    [DataContract]
    public class ProductConfig 
    {
        [DataMember(Name = "Id")]
        private string _id;
        [DataMember(Name = "Currency")]
        public Currency Currency;
        [DataMember(Name = "Cost")]
        public int Cost;
        public string Id => _id;
    }
}