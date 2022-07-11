using UnityEngine;

namespace Dino.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        public void PlayDeath()
        {
            Destroy(gameObject);
        }
    }
}
