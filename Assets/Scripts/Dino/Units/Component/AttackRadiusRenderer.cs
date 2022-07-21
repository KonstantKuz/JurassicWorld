using Dino.Weapon.Model;
using Feofun.Components;
using UnityEngine;

namespace Dino.Units.Component
{
    public class AttackRadiusRenderer : MonoBehaviour, IInitializable<IWeaponModel>, IUpdatableComponent
    {
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _initialRadius;
        [SerializeField] private GameObject _prefab;

        private Transform _plane;
        private Transform Plane => _plane ??= Instantiate(_prefab).transform;
        public void Init(IWeaponModel owner)
        {
            Plane.localScale = Vector3.one * _initialRadius * owner.AttackDistance;
        }

        public void OnTick()
        {
            Plane.position = transform.position;
            Plane.rotation *= Quaternion.Euler(0,_rotationSpeed * Time.deltaTime,0);
        }
    }
}
