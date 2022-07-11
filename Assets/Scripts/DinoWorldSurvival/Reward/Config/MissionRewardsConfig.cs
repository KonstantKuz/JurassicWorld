using System.Runtime.Serialization;
using DinoWorldSurvival.Session.Model;
using Feofun.Config;

namespace DinoWorldSurvival.Reward.Config
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