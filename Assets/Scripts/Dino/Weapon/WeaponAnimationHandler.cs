using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Weapon
{
    public class WeaponAnimationHandler : MonoBehaviour
    {
        public Action OnFireEvent;

        [UsedImplicitly]
        public void Fire()
        {
            OnFireEvent?.Invoke();
        }
    }
}