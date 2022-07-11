using DinoWorldSurvival.Player.Inventory.Service;
using DinoWorldSurvival.Player.Progress.Service;
using DinoWorldSurvival.Player.Wallet;
using Zenject;

namespace DinoWorldSurvival.Player.Installer
{
    public class PlayerServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<PlayerProgressService>().AsSingle();
            container.Bind<PlayerProgressRepository>().AsSingle();    
            
            container.Bind<WalletService>().AsSingle();            
            container.Bind<WalletRepository>().AsSingle();
            
            container.Bind<InventoryService>().AsSingle();
            container.Bind<InventoryRepository>().AsSingle();       
            
        }
    }
}
