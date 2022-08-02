using System.Collections.Generic;
using System.Linq;
using Dino.Player.Wallet;
using Dino.Reward.Model;
using Dino.Session.Model;

namespace Dino.UI.Screen.Debriefing.Model
{
    public class DebriefingScreenModel
    {
        public readonly SessionResult SessionResult;
        public readonly Session.Model.Session Session;

        public DebriefingScreenModel(SessionResult sessionResult, Session.Model.Session session)
        {
            SessionResult = sessionResult;
            Session = session;
        }

        public ResultPanelModel BuildResultPanelModel(List<RewardItem> rewards)
        {
            var coinsCount = rewards.First(it => it.RewardId == Currency.Soft.ToString()).Count;
            return new ResultPanelModel
            {
                SessionResult = SessionResult,
                KillCount = Session.Kills,
                CoinsCount = coinsCount,
            };
        }
    }
}