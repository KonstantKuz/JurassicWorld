using Dino.Squad.Progress;
using Dino.Squad.Service;
using Zenject;

namespace Dino.Squad.Installer
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
