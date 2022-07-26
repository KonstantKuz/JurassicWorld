using System;
using Dino.Extension;
using Dino.Units.Component;
using Feofun.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMovement : MonoBehaviour, IMovementController, IInitializable<Unit>
    {
        [SerializeField] private float _rotationSpeed;
        
        private NavMeshAgent _agent;
        
        public bool IsStopped
        {
            get => _agent.isStopped;
            set => _agent.isStopped = value;
        }
        
        public void Init(Unit owner)
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.speed = owner.Model.MoveSpeed;
        }

        public void MoveTo(Vector3 position)
        {
            IsStopped = false;
            _agent.SetDestination(position);
        }

        public void RotateTo(Vector3 position, float overrideRotationSpeed = 0f)
        {
            var lookAtDirection = (position - transform.position).XZ().normalized;
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            var finalSpeed = Math.Abs(overrideRotationSpeed) > Mathf.Epsilon ? overrideRotationSpeed : _rotationSpeed; 
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * finalSpeed);
        }
    }
}