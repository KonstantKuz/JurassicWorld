using System.Runtime.Serialization;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class IngredientConfig
    {
        [DataMember(Name = "Ingredient")]
        public string Name;     
        [DataMember(Name = "Count")]
        public int Count;
    }
}