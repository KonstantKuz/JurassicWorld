using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Camera;
using Dino.Units.Player;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using AppContext = Feofun.App.AppContext;

namespace Dino.Location
{
    public class World : RootContainer
    {
        [SerializeField]
        private Transform _ground;
        [SerializeField]
        private GameObject _spawn;
        [SerializeField]
        private CameraController _cameraController;

        public Transform Ground => _ground;
        public GameObject Spawn => _spawn;

        public bool IsPaused => Time.timeScale == 0;
        
        [CanBeNull]
        public Level.Level Level { get; private set; }
        [CanBeNull]
        public PlayerUnit Player { get; private set; }
        
        public CameraController CameraController => _cameraController;

        public void SetLevel(Level.Level level)
        {
            Level = level;
        }
        
        public void SetPlayer(PlayerUnit playerUnit)
        {
            Player = playerUnit;
            CameraController.Target = Player?.transform;
        }

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }

        public void Setup()
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldSetup());
        }

        public void CleanUp()
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldCleanUp());
        }

        private IEnumerable<T> GetAllOf<T>()
        {
            return GetDISubscribers<T>().Union(GetChildrenSubscribers<T>());
        }
        private static List<T> GetDISubscribers<T>() => AppContext.Container.ResolveAll<T>();
    }
}