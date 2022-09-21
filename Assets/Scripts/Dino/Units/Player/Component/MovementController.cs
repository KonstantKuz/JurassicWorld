using System;
using Dino.Extension;
using Dino.Units.Component;
using Feofun.Components;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementController : MonoBehaviour, IMovementController, IInitializable<Unit>, IUpdatableComponent, IUnitDeathEventReceiver, IUnitDeactivateEventReceiver
    {
        private readonly int _runHash = Animator.StringToHash("Run");
        private readonly int _idleHash = Animator.StringToHash("Idle");

        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _horizontalMotionHash = Animator.StringToHash("HorizontalMotion");

        [Inject] private Joystick _joystick;
        
        [SerializeField]
        private float _rotationSpeed = 10;
        
        private Animator _animator;
        private NavMeshAgent _agent;
        
        public bool IsStopped
        {
            get => _agent.isStopped;
            set => _agent.isStopped = value;
        }
        public bool IsMoving => _joystick.Direction.sqrMagnitude > 0;
        public Vector3 MoveDirection => new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);
        public bool HasTarget { get; private set; }

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public void Init(Unit unit)
        {
            _agent.speed = unit.Model.MoveSpeed;
        }
        
        public void OnTick()
        {
            IsStopped = !IsMoving;
            MoveTo(transform.position + MoveDirection);
            UpdateAnimation();
            if (HasTarget) return;
            RotateTo(transform.position + MoveDirection, _rotationSpeed);
        }

        public void MoveTo(Vector3 position)
        {
            _agent.SetDestination(position);
        }

        public void Warp(Vector3 position)
        {
            _agent.Warp(position);
        }

        private void UpdateAnimation()
        {
            PlayAnimation(IsMoving);
            UpdateAnimationRotateValues(MoveDirection);

        }

        private void PlayAnimation(bool isMoving)
        {
            _animator.Play(isMoving ? _runHash : _idleHash);
        }

        public void RotateTo(Vector3 position, float rotationSpeed)
        {
            var lookAtDirection = (position - transform.position).XZ().normalized;
            if (lookAtDirection == Vector3.zero) { return; }
            var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * _rotationSpeed);
        }
        public void RotateToTarget([CanBeNull] Transform target)
        {
            if (target != null) {
                HasTarget = true;
                RotateTo(target.position, _rotationSpeed);
            } else {
                HasTarget = false;
            }
        }
        public void OnDeactivate()
        {
            StopAnimation();
        }
        public void OnDeath(DeathCause deathCause)
        {
            StopAnimation();
        }

        private void StopAnimation()
        {
            _animator.Play(_idleHash);
            _animator.SetFloat(_horizontalMotionHash, 0);
            _animator.SetFloat(_verticalMotionHash, 0);
        }

        private void UpdateAnimationRotateValues(Vector3 moveDirection)
        {
            if (moveDirection.sqrMagnitude <= 0) {
                _animator.SetFloat(_horizontalMotionHash, 0);
                _animator.SetFloat(_verticalMotionHash, 0);
                return;
            }
            var signedAngle = GetRotateSignedAngle(moveDirection);
            _animator.SetFloat(_horizontalMotionHash, (float) Math.Sin(GetRadian(signedAngle)));
            _animator.SetFloat(_verticalMotionHash, (float) Math.Cos(GetRadian(signedAngle)));
        }
        private double GetRadian(float signedAngle) => Mathf.Deg2Rad * signedAngle;
        private float GetRotateSignedAngle(Vector3 moveDirection) => Vector2.SignedAngle(transform.forward.ToVector2XZ(), moveDirection.ToVector2XZ());
       
    }
}
