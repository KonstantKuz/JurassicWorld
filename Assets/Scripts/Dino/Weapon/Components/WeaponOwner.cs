using UnityEngine;

namespace Dino.Weapon.Components
{
    public class WeaponOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _barrel;
        
        public Transform Barrel => _barrel;
    }
}