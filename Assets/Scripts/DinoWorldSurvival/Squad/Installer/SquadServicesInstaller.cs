using DinoWorldSurvival.Squad.Progress;
using DinoWorldSurvival.Squad.Service;
using Zenject;

namespace DinoWorldSurvival.Squad.Installer
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SquadFactory>().AsSingle();
            container.BindInterfacesAndSelfTo<SquadProgressService>().AsSingle();

            container.Bind<SquadProgressRepository>().AsSingle();
        }
    }
}
