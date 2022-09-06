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

        private PlayerUnit _player;
        public Transform Ground => _ground;
        public GameObject Spawn => _spawn;
        public bool IsPaused => Time.timeScale == 0;
        public CameraController CameraController => _cameraController;

        [CanBeNull]
        public Level.Level Level { get; set; }
        [CanBeNull]
        public PlayerUnit Player
        {
            get => _player;
            set
            {
                _player = value;
                CameraController.Target = value?.transform;
            }
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