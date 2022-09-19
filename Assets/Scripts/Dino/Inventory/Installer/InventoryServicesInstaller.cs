using Dino.Inventory.Service;
using Dino.Weapon.Service;
using Zenject;

namespace Dino.Inventory.Installer
{
    public class InventoryServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
            container.Bind<CraftService>().AsSingle();
            container.BindInterfacesAndSelfTo<LevelInitialInventoryService>().AsSingle();
        }
    }
}