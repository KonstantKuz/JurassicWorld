using System.Collections.Generic;
using Dino.Player.Wallet;
using Dino.Reward.Config;
using Dino.Reward.Model;
using Dino.Session.Model;
using Feofun.Config;
using JetBrains.Annotations;
using Zenject;

namespace Dino.Reward.Service
{
    [PublicAPI]
    public class MissionResultRewardService
    {
        [Inject]
        private readonly ConfigCollection<SessionResult, MissionRewardsConfig> _missionRewards;

        public List<RewardItem> CalculateRewards(SessionResult result, Session.Model.Session session)
        {
            var missionConfig = _missionRewards.Get(result);
            var rewardCount = (int) (session.Kills * missionConfig.KilledFactor);
            var reward = new RewardItem(Currency.Soft.ToString(), RewardType.Currency, rewardCount);
            return new List<RewardItem>() {
                    reward
            };
        }
    }
}