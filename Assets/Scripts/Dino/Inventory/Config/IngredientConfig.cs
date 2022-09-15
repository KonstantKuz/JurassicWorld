using System.Runtime.Serialization;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class IngredientConfig
    {
        [DataMember(Name = "IngredientId")]
        public string Id;     
        [DataMember(Name = "Count")]
        public int Count;
    }
}