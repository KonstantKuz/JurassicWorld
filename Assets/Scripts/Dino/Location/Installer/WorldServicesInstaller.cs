﻿using Dino.Location.Level.Service;
using Dino.Location.Service;
using Dino.Session.Service;
using Dino.Units.Service;
using UnityEngine;
using Zenject;

namespace Dino.Location.Installer
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
  
            container.Bind<LevelService>().AsSingle();
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
            container.BindInterfacesAndSelfTo<DirectionNavigatorService>().AsSingle();
        }
    }
}