using Dino.Inventory.Service;
using Zenject;

namespace Dino.Inventory.Installer
{
    public class InventoryServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
            container.Bind<InventoryRepository>().AsSingle();
            
            container.Bind<CraftService>().AsSingle();
        }
    }
}