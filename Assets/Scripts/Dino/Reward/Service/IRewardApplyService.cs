using System.Collections.Generic;
using Dino.Reward.Model;

namespace Dino.Reward.Service
{
    public interface IRewardApplyService
    {
        void ApplyReward(RewardItem rewardItem);
        public void ApplyRewards(IEnumerable<RewardItem> items);
    }
}