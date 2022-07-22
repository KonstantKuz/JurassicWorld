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
        
        public void Init(IWeaponModel owner)
        {
            if (_plane == null) {
                _plane = CreatePlane();
            }
            _plane.gameObject.SetActive(true);
            _plane.localScale = Vector3.one * _initialRadius * owner.AttackDistance;
        }

        private Transform CreatePlane() => _worldObjectFactory.CreateObject(_prefab).transform;

        public void OnTick()
        {
            if (_plane == null) {
                return;
            }
            _plane.position = transform.position;
            _plane.rotation *= Quaternion.Euler(0,_rotationSpeed * Time.deltaTime,0);
        }

        public void Dispose()
        {
            if (_plane == null) {
                return;
            }
            _plane.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (_plane == null) {
                return;
            }
            Destroy(_plane.gameObject);
        }
    }
}
