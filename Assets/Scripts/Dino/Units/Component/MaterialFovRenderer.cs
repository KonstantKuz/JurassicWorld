using UnityEngine;

namespace Dino.Units.Component
{
    public class MaterialFovRenderer : MonoBehaviour, IFieldOfViewRenderer
    {
        [SerializeField] private ConeOfSightRenderer _coneOfSightRenderer;

        public void Init(float angle, float radius)
        {
            _coneOfSightRenderer.ViewAngle = angle;
            _coneOfSightRenderer.ViewDistance = radius;
            _coneOfSightRenderer.Init();
        }
    }
}