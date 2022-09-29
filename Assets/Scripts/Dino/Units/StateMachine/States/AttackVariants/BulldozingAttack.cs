using System;
using DG.Tweening;
using Dino.Location;
using Dino.Units.Component;
using Dino.Units.Enemy.Config;
using Dino.Units.Enemy.Model.EnemyAttack;
using Dino.Weapon.Components;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        public class BulldozingAttack : AttackStateBase
        {
            private const string ATTACK_LINE_PREFAB = "BulldozingAttackLine";

            private readonly BulldozingAttackModel _bulldozingAttackModel;
            private readonly WeaponTimer _attackTimer;
            private readonly float _animationSpeedMultiplier;

            private Tween _attackTween;
            private Trigger _damageTrigger;
            private Collider _collider;
            private LineRenderer _attackIndicator;
            private IFieldOfViewRenderer _fovRenderer;
            private bool _isSafeTimeStarted;

            private Collider Collider => _collider ??= Owner.gameObject.GetComponent<Collider>();
            private Trigger DamageTrigger => _damageTrigger ??= GetOrAddSelfDamageTrigger();
            private float PlayerSafeTime => _bulldozingAttackModel.SafeTime; // time between dino stopped aiming on player and actual attack jump
            private bool IsAttacking => _attackTween != null;
            private bool IsAttackReady => _attackTimer.IsAttackReady.Value;
            private Vector3 AttackPosition => Owner.transform.position + Owner.transform.forward * AttackModel.AttackDistance;

            public BulldozingAttack(UnitStateMachine stateMachine, EnemyAttackModel attackModel, Action<GameObject> hitCallback) 
                : base(stateMachine, attackModel, hitCallback)
            {
                _bulldozingAttackModel = attackModel.CreateCurrentVariantModel<BulldozingAttackModel>();
                _attackTimer = new WeaponTimer(AttackModel.AttackInterval);
                _animationSpeedMultiplier = _bulldozingAttackModel.Speed / Owner.Model.MoveSpeed;
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
                if (SwitchToPatrolStateIfShould()) return;

                if (!_isSafeTimeStarted)
                {
                    StateMachine._movementController.RotateTo(TargetPosition, _bulldozingAttackModel.RotationSpeed);
                }
                
                if (SwitchToChaseStateIfShould()) return;
                if (!IsAttackReady)
                {
                    PrepareToAttack();
                    return;
                }
                
                Attack();
            }

            private void PrepareToAttack()
            {
                if (_attackTimer.ReloadTimeLeft > PlayerSafeTime) return;

                StateMachine._animationWrapper.PlayMoveForwardSmooth();
                var animationSpeed = 1f - _attackTimer.ReloadTimeLeft / _bulldozingAttackModel.SafeTime;
                StateMachine._animationWrapper.SetSpeed(animationSpeed * _animationSpeedMultiplier);
                
                if(_isSafeTimeStarted) return;
                _isSafeTimeStarted = true;
                CreateAttackIndicator();
                _fieldOfViewRenderer?.SetActive(false);
            }

            private void CreateAttackIndicator()
            {
                _attackIndicator = StateMachine._worldObjectFactory.CreateObject(ATTACK_LINE_PREFAB).RequireComponent<LineRenderer>();
                _attackIndicator.SetPositions(new [] {Owner.transform.position, AttackPosition});
            }

            private void Attack()
            {
                _attackTween = PlayBulldozing();
                _attackTween.onComplete = OnCompleteBulldozing;
                
                EnableAttackCollision();
                if (!IsTargetInvalid)
                {
                    Target.OnTargetInvalid += Dispose;
                }
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
                var moveDuration = distance / _bulldozingAttackModel.Speed;
                return Owner.transform.DOMove(hit.position, moveDuration).SetEase(Ease.Linear);
            }

            private void OnCompleteBulldozing()
            {
                StateMachine._movementController.Warp(Owner.transform.position);
                Dispose();
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
                _isSafeTimeStarted = false;
                _fieldOfViewRenderer?.SetActive(true);
 
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