using System.Runtime.Serialization;
using Dino.Inventory.Model;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class ProvidedItemConfig
    {
        [DataMember] 
        public string ItemId;
        [DataMember] 
        public InventoryItemType Type;
        [DataMember]
        public int Amount;
    }
}