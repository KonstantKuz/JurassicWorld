using System.Runtime.Serialization;
using Dino.Inventory.Model;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class CraftItemConfig
    {
        public string Id { get; set; }

        [DataMember(Name = "ItemType")]
        public InventoryItemType Type;

        [DataMember(Name = "ItemCount")]
        public int Count;

        public static CraftItemConfig Create(string id, CraftItemConfig config)
        {
            return new CraftItemConfig() {
                    Id = id,
                    Type = config.Type,
                    Count = config.Count
            };
        }
    }
}