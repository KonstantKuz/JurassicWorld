using UnityEngine;

namespace DinoWorldSurvival.Units.Component.Death
{
    public class DestroyDeath : MonoBehaviour, IUnitDeath
    {
        public void PlayDeath()
        {
            Destroy(gameObject);
        }
    }
}
