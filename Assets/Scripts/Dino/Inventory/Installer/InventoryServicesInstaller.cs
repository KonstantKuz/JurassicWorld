using Dino.Inventory.Service;
using Dino.Weapon.Service;
using Zenject;
using Dino.Loot.Service;

namespace Dino.Inventory.Installer
{
    public class InventoryServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
            container.Bind<CraftService>().AsSingle();
            container.BindInterfacesAndSelfTo<LootService>().AsSingle();
            container.BindInterfacesAndSelfTo<LootRespawnService>().AsSingle();
            container.BindInterfacesAndSelfTo<LevelInitialInventoryService>().AsSingle();
        }
    }
}