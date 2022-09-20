using Zenject;

namespace Dino.Advertisment
{
    public class AdsServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            Feofun.Advertisment.Installer.AdsServicesInstaller.Install(container);
        }
    }
}