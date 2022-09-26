using UnityEngine;

namespace Feofun.UI
{
    public class UIRoot : MonoBehaviour
    {
        [field:SerializeField] public Transform HudContainer { get; private set; }    
        [field:SerializeField] public Transform FlyingIconContainer { get; private set; }
    }
}
