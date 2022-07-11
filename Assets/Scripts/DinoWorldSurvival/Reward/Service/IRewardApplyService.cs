using System.Collections.Generic;
using DinoWorldSurvival.Reward.Model;

namespace DinoWorldSurvival.Reward.Service
{
    public interface IRewardApplyService
    {
        void ApplyReward(RewardItem rewardItem);
        public void ApplyRewards(IEnumerable<RewardItem> items);
    }
}