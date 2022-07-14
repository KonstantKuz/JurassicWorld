using UnityEngine;

namespace Dino.Units.Component
{
    public interface IMovementController
    {
        bool IsStopped { get; set; }
        void MoveTo(Vector3 position);
        void RotateTo(Vector3 position);
    }
}