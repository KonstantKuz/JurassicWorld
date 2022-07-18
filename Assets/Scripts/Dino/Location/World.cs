using System;
using System.Collections.Generic;
using System.Linq;
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

        public Transform Ground => _ground;
        public GameObject Spawn => _spawn;

        public bool IsPaused => Time.timeScale == 0;
       
        [CanBeNull]
        public PlayerUnit Player { get; set; }
        
        public PlayerUnit GetPlayer() 
        {
            if (Player == null) {
                throw new NullReferenceException("Player is null, should call this method only inside game session");
            }

            return Player;
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