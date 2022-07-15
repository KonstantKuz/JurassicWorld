using Dino.Location.Service;
using Dino.Loot.Service;
using Dino.Session.Service;
using Dino.Units.Service;
using UnityEngine;
using Zenject;

namespace Dino.Location
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private WorldObjectFactory _worldObjectFactory;
        [SerializeField] private EnemyInitService _enemyInitService;

        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.BindInterfacesAndSelfTo<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<World>().FromInstance(_world);
            container.Bind<EnemyInitService>().FromInstance(_enemyInitService);
  
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
            
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
        }
    }
}