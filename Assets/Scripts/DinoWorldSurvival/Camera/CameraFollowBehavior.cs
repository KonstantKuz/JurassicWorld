using System;
using DG.Tweening;
using Feofun.Components;
using UniRx;
using UnityEngine;
using Unit = DinoWorldSurvival.Units.Unit;

namespace DinoWorldSurvival.Camera
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