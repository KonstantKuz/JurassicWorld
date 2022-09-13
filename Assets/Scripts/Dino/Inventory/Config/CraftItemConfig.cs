using System.Runtime.Serialization;
using Dino.Inventory.Model;

namespace Dino.Inventory.Config
{
    [DataContract]
    public class CraftItemConfig
    {
        public string Id { get; set; }

        [DataMember(Name = "CraftItemType")]
        public InventoryItemType Type;

        [DataMember(Name = "CraftItemCount")]
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