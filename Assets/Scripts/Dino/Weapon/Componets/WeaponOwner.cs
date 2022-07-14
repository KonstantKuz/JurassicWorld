using UnityEngine;

namespace Dino.Weapon.Componets
{
    public class WeaponOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _barrel;

        public Transform Barrel => _barrel;
    }
}