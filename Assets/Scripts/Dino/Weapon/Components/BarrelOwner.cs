using UnityEngine;

namespace Dino.Weapon.Components
{
    public class BarrelOwner : MonoBehaviour
    {
        [SerializeField]
        private Transform _barrel;
        
        public Transform Barrel => _barrel;
    }
}