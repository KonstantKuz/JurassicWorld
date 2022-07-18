using System;
using UnityEngine;

namespace Dino.Location
{
    public class Trigger : MonoBehaviour
    {
        public event Action<Collider> OnTriggerEnterCallback;

        public void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterCallback?.Invoke(other);
        }
    }
}
