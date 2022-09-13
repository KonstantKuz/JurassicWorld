using Feofun.Advertisment.Providers;
using Feofun.Advertisment.Service;
using Zenject;

namespace Feofun.Advertisment.Installer
{
    public class AdsServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<IAdsProvider>().To<YCAdsProviderAdapter>().AsSingle();
            container.Bind<AdsManager>().AsSingle();
        }
    }
}