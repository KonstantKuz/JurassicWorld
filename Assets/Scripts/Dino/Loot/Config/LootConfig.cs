using System.Runtime.Serialization;
using Feofun.Config;

namespace Dino.Loot.Config
{
    [DataContract]
    public class LootConfig : ICollectionItem<string>
    {
        [DataMember(Name = "Id")] 
        private string _id;
        
        public string Id => _id;
        [DataMember]
        public string ReceivedItemId;
    }
}