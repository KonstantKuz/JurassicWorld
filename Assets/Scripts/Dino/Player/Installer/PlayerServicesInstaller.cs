using Dino.Player.Progress.Service;
using Dino.Player.Wallet;
using Zenject;

namespace Dino.Player.Installer
{
    public class PlayerServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<PlayerProgressService>().AsSingle();
            container.Bind<PlayerProgressRepository>().AsSingle();    
            
            container.Bind<WalletService>().AsSingle();            
            container.Bind<WalletRepository>().AsSingle();
        }
    }
}
