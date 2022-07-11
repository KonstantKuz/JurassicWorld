using Dino.Reward.Service;
using Zenject;

namespace Dino.Reward.Installer
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