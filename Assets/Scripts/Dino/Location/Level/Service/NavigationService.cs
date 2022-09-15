using Dino.Location.Service;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Dino.Location.Level.Service
{
    public class NavigationService : IWorldScope
    {
        private NavigationArrow _navigationArrow;

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        public NavigationArrow NavigationArrow => _navigationArrow ??= CreateNavigationArrow();

        public void OnWorldSetup()
        {
        }

        private NavigationArrow CreateNavigationArrow()
        {
            var arrow = NavigationArrow.Spawn(_worldObjectFactory);
            arrow.Parent = _world.RequirePlayer().transform;
            return arrow;
        }

        public void NavigatePlayerTo([CanBeNull]Transform target)
        {
            NavigationArrow.gameObject.SetActive(target != null);
            NavigationArrow.Target = target;
        }

        public void OnWorldCleanUp()
        {
            _navigationArrow = null;
        }
    }
}