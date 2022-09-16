using Dino.Location.Service;
using UnityEngine;
using Zenject;

namespace Dino.Location.Level.Service
{
    public class NavigationService : IWorldScope
    {
        private NavigationArrow _navigationArrow;

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;

        private NavigationArrow NavigationArrow => _navigationArrow ??= CreateNavigationArrow();

        public void OnWorldSetup()
        {
        }

        private NavigationArrow CreateNavigationArrow()
        {
            var arrow = NavigationArrow.Spawn(_worldObjectFactory);
            arrow.Parent = _world.RequirePlayer().transform;
            return arrow;
        }

        public void PointNavArrowAt(Transform target)
        {
            NavigationArrow.gameObject.SetActive(true);
            NavigationArrow.Target = target;
        }

        public void HideNavArrow()
        {
            NavigationArrow.gameObject.SetActive(false);
            NavigationArrow.Target = null;
        }

        public void OnWorldCleanUp()
        {
            _navigationArrow = null;
        }
    }
}