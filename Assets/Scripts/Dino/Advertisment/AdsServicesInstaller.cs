using Zenject;

namespace Dino.Advertisment
{
    public class AdsServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<AdsEventHandler>().AsSingle().NonLazy();
            Feofun.Advertisment.Installer.AdsServicesInstaller.Install(container);
        }
    }
}