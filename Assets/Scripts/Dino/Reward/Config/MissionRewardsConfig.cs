using System.Runtime.Serialization;
using Dino.Session.Model;
using Feofun.Config;

namespace Dino.Reward.Config
{
    [DataContract]
    public class MissionRewardsConfig : ICollectionItem<SessionResult>
    {
        [DataMember(Name = "Result")]
        public SessionResult Result;

        [DataMember(Name = "KilledFactor")]
        public float KilledFactor;
        public SessionResult Id => Result;
    }
}