using UnityEngine;

namespace Dino.Tutorial
{
    public class WorkbenchTutorialItem : MonoBehaviour
    {
        [SerializeField] private int _pointAtOrder;
        public int PointAtOrder => _pointAtOrder;
    }
}