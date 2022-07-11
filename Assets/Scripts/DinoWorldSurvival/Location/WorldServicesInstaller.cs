using DinoWorldSurvival.Enemy.Spawn;
using DinoWorldSurvival.Location.Service;
using DinoWorldSurvival.Loot.Service;
using DinoWorldSurvival.Session.Service;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.Location
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private WorldObjectFactory _worldObjectFactory;
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        [SerializeField] private EnemyHpsSpawner _enemyHpsSpawner;
        
        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.BindInterfacesAndSelfTo<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<World>().FromInstance(_world);
            
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.BindInterfacesAndSelfTo<ReviveService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
            
            container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.Bind<EnemyHpsSpawner>().FromInstance(_enemyHpsSpawner).AsSingle();
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
        }
    }
}