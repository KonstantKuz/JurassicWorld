using System;
using DG.Tweening;
using Dino.Extension;
using Dino.Location;
using Dino.Units.Enemy.Model.EnemyAttack;
using Dino.Weapon.Components;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        public class BulldozingAttack : AttackSubState
        {
            private const string ATTACK_LINE_PREFAB = "BulldozingAttackLine";
            
            private readonly WeaponTimer _attackTimer;
            private readonly float _animationSpeedMultiplier;

            private Tween _attackTween;
            private Trigger _damageTrigger;
            private Collider _collider;
            private LineRenderer _attackIndicator;
            private bool _isSafeTimeStarted;

            private Collider Collider => _collider ??= Owner.gameObject.GetComponent<Collider>();
            private Trigger DamageTrigger => _damageTrigger ??= GetOrAddSelfDamageTrigger();
            private bool IsAttacking => _attackTween != null;
            private bool IsAttackReady => _attackTimer.IsAttackReady.Value;
            private Vector3 AttackPosition => Owner.transform.position + Owner.transform.forward * AttackModel.AttackDistance;

            public BulldozingAttack(UnitStateMachine stateMachine, EnemyAttackModel attackModel, Action<GameObject> hitCallback) 
                : base(stateMachine, attackModel, hitCallback)
            {
                _attackTimer = new WeaponTimer(AttackModel.AttackInterval);
                _animationSpeedMultiplier = AttackModel.Bulldozing.Speed / Owner.Model.MoveSpeed;
            }

            private Trigger GetOrAddSelfDamageTrigger()
            {
                return Owner.gameObject.GetComponent<Trigger>() ?? Owner.gameObject.AddComponent<Trigger>();
            }

            public override void OnEnterState()
            {
                Dispose();
                
                StateMachine._animationWrapper.PlayIdleSmooth();
                StateMachine._movementController.IsStopped = true;
                StateMachine._owner.OnDeath += (unit, cause) => Dispose();
            }

            public override void OnTick()
            {
                if (IsAttacking) return;
                if (IsRequiredPatrolState()) return;

                StateMachine._movementController.RotateTo(TargetPosition, AttackModel.Bulldozing.RotationSpeed);
                UpdateAttackIndicator();
                
                if (IsRequiredChaseState()) return;
                if (!IsAttackReady)
                {
                    PrepareToAttack();
                    return;
                }
                
                Attack();
            }

            private void UpdateAttackIndicator()
            {
                if(_attackIndicator == null) return;
                
                _attackIndicator.SetPosition(1, AttackPosition);
            }

            private void PrepareToAttack()
            {
                StateMachine._animationWrapper.PlayMoveForwardSmooth();
                StateMachine._animationWrapper.SetSpeed(_attackTimer.ReloadProgress * _animationSpeedMultiplier);
                
                if(_attackIndicator != null) return;
                
                CreateAttackIndicator();
            }

            private void CreateAttackIndicator()
            {
                _attackIndicator = StateMachine._worldObjectFactory.CreateObject(ATTACK_LINE_PREFAB).RequireComponent<LineRenderer>();
                _attackIndicator.SetPositions(new [] {Owner.transform.position, AttackPosition});
            }

            private void Attack()
            {
                EnableAttackCollision();
                _attackTween = PlayBulldozing();
                _attackTween.onComplete = OnCompleteBulldozing;
                
                if (!IsTargetInvalid)
                {
                    Target.OnTargetInvalid += Dispose;
                }
            }

            private void OnCompleteBulldozing()
            {
                StateMachine._movementController.Warp(Owner.transform.position);
                Dispose();
            }
            
            private void EnableAttackCollision()
            {
                Collider.isTrigger = true;
                DamageTrigger.OnTriggerEnterCallback += TryHit;
            }

            private Tween PlayBulldozing()
            {
                NavMesh.SamplePosition(AttackPosition, out var hit, AttackModel.AttackDistance, NavMesh.AllAreas);
                
                var distance = (Owner.transform.position - hit.position).magnitude;
                var moveDuration = distance / AttackModel.Bulldozing.Speed;
                return Owner.transform.DOMove(hit.position, moveDuration).SetEase(Ease.Linear);
            }

            public override void OnExitState()
            {
                Dispose();
            }

            private void Dispose()
            {
                _attackTimer.OnAttack();
                _attackTween?.Kill();
                _attackTween = null;
                
                DeleteAttackIndicator();
                ResetAttackAnimation();
                DisableAttackCollision();
                
                StateMachine._owner.OnDeath -= (unit, cause) => Dispose();
                
                if (!IsTargetInvalid)
                {
                    Target.OnTargetInvalid -= Dispose;
                }
            }

            private void DeleteAttackIndicator()
            {
                if (_attackIndicator == null) return;
                
                Destroy(_attackIndicator.gameObject);
                _attackIndicator = null;
            }
            
            private void ResetAttackAnimation()
            {
                StateMachine._animationWrapper.ResetSpeed();
                StateMachine._animationWrapper.PlayIdleSmooth();
            }

            private void DisableAttackCollision()
            {
                Collider.isTrigger = false;
                DamageTrigger.OnTriggerEnterCallback -= TryHit;
            }
        }   
    }
}