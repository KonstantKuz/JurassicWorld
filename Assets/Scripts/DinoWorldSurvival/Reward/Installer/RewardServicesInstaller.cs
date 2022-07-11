using DinoWorldSurvival.Reward.Service;
using Zenject;

namespace DinoWorldSurvival.Reward.Installer
{
    public class RewardServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<MissionResultRewardService>().AsSingle();
            container.Bind<IRewardApplyService>().To<RewardApplyService>().AsSingle();
        }
    }
}