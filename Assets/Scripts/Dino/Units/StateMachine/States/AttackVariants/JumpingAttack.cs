using System;
using DG.Tweening;
using Dino.Units.Enemy.Model.EnemyAttack;
using Dino.Weapon.Components;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        public class JumpingAttack : AttackSubState
        {
            private const string ATTACK_RADIUS_PREFAB = "JumpingAttackRadius";
            private const string ATTACK_VFX_PREFAB = "JumpingAttackVfx";
            
            private readonly WeaponTimer _attackTimer;

            private Tween _attackTween;
            private GameObject _attackIndicator;
            private Tween _indicatorScale;
            private GameObject _attackVfx;
            private bool _isSafeTimeStarted;

            private bool IsAttacking => _attackTween != null;
            private bool IsAttackReady => _attackTimer.IsAttackReady.Value;
            private Vector3 AttackPosition { get; set; }
            
            private float PlayerSafeTime => AttackModel.Jumping.PlayerSafeTime; // time between dino stopped aiming on player and actual attack jump
            private float JumpHeight => AttackModel.Jumping.Height;
            private float JumpDuration => AttackModel.Jumping.Duration;
            private float DamageRadius => AttackModel.Jumping.DamageRadius;

            public JumpingAttack(UnitStateMachine stateMachine, EnemyAttackModel attackModel, Action<GameObject> hitCallback) 
                : base(stateMachine, attackModel, hitCallback)
            {
                _attackTimer = new WeaponTimer(AttackModel.AttackInterval);
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

                if (!_isSafeTimeStarted)
                {
                    StateMachine._movementController.RotateTo(TargetPosition, AttackModel.Bulldozing.RotationSpeed);
                    AttackPosition = TargetPosition;
                }
                
                if (IsRequiredChaseState()) return;
                if (!IsAttackReady)
                {
                    PrepareToAttack();
                    return;
                }
                
                Attack();
            }

            protected override bool IsRequiredChaseState()
            {
                if (IsTargetInAttackRange) return false;
                StateMachine.SetState(UnitState.Chase);
                return true;
            }

            private void PrepareToAttack()
            {
                if (_attackTimer.ReloadTimeLeft > AttackModel.Jumping.PlayerSafeTime) return;
                if(_isSafeTimeStarted) return;
                _isSafeTimeStarted = true;
                CreateAttackIndicator();
            }

            private void CreateAttackIndicator()
            {
                _attackIndicator = StateMachine._worldObjectFactory.CreateObject(ATTACK_RADIUS_PREFAB);
                _attackIndicator.transform.position = TargetPosition;
                _attackIndicator.transform.localScale = Vector3.zero;
                _indicatorScale = _attackIndicator.transform.DOScale(Vector3.one * AttackModel.Jumping.DamageRadius, PlayerSafeTime);
            }
            
            private void Attack()
            {
                _attackTween = PlayJump();
                _attackTween.onComplete = OnCompleteJump;
                
                if (!IsTargetInvalid)
                {
                    Target.OnTargetInvalid += Dispose;
                }
            }

            private void OnCompleteJump()
            {
                StateMachine._movementController.Warp(Owner.transform.position);

                SpawnDamageVfx();
                HitObjectsAround();
                Dispose();
            }

            private void SpawnDamageVfx()
            {
                var vfx = StateMachine._worldObjectFactory.CreateObject(ATTACK_VFX_PREFAB);
                vfx.transform.position = Owner.transform.position;
            }

            private void HitObjectsAround()
            {
                var colliders = Physics.OverlapSphere(Owner.transform.position, DamageRadius, StateMachine._layerMaskProvider.DamageMask);
                colliders.ForEach(TryHit);
            }

            private Tween PlayJump()
            {
                return Owner.transform.DOJump(AttackPosition, JumpHeight, 1, JumpDuration).SetEase(Ease.Linear);
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
                
                DeleteAttackIndicator();
                
                StateMachine._owner.OnDeath -= (unit, cause) => Dispose();

                if (!IsTargetInvalid)
                {
                    Target.OnTargetInvalid -= Dispose;
                }
            }

            private void DeleteAttackIndicator()
            {
                if(_attackIndicator == null) return;
                
                _indicatorScale?.Kill();
                _indicatorScale = null;
                
                Destroy(_attackIndicator.gameObject);
                _attackIndicator = null;
            }
        }
    }
}