using UnityEngine;

namespace Dino.Camera
{
    public class CameraFollowBehavior : MonoBehaviour
    {
        [SerializeField] 
        private float _distance;

        private void Update()
        {
            var cameraTransform = UnityEngine.Camera.main.transform;
            cameraTransform.position = transform.position - _distance * cameraTransform.forward;
        }
    }
}