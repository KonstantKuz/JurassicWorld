using Dino.Inventory.Service;
using Dino.Units.Component.Target;
using Dino.Units.Service;
using Dino.Weapon.Service;
using Zenject;

namespace Dino.Units.Installer
{
    public class UnitServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<UnitFactory>().AsSingle();
            container.Bind<TargetService>().AsSingle();
            container.Bind<UnitService>().AsSingle();     
            container.Bind<PlayerUnitModelBuilder>().AsSingle();    
   
            container.Bind<WeaponService>().AsSingle();
            container.Bind<ActiveItemService>().AsSingle();   
            
            container.BindInterfacesAndSelfTo<InventoryService>().AsSingle();
            container.Bind<InventoryRepository>().AsSingle();    
        }
    }
}