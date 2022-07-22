using System;
using Dino.Location.Service;
using Dino.Weapon.Model;
using Feofun.Components;
using UnityEngine;
using Zenject;

namespace Dino.Units.Component
{
    public class AttackRadiusRenderer : MonoBehaviour, IInitializable<IWeaponModel>, IUpdatableComponent, IDisposable
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _initialRadius;
        [SerializeField] private GameObject _prefab;

        private Transform _plane;

        [Inject] private WorldObjectFactory _worldObjectFactory;

        private Transform Plane => _plane ??= _worldObjectFactory.CreateObject(_prefab).transform;
        
        public void Init(IWeaponModel owner)
        {
            Plane.gameObject.SetActive(true);
            Plane.localScale = Vector3.one * _initialRadius * owner.AttackDistance;
        }

        public void OnTick()
        {
            Plane.position = transform.position;
            Plane.rotation *= Quaternion.Euler(0,_rotationSpeed * Time.deltaTime,0);
        }

        public void Dispose()
        {
            Plane.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Destroy(Plane.gameObject);
        }
    }
}
