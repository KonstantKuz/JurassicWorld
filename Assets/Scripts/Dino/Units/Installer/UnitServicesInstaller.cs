using Dino.Inventory.Service;
using Dino.Units.Service;
using Dino.Units.Target;
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
            
            container.Bind<InventoryApplyService>().AsSingle();
        }
    }
}