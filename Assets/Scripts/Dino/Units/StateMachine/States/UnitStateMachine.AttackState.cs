﻿using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Model;
using Dino.Units.Player.Attack;
using Dino.Units.Target;
using Dino.Units.Weapon;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class AttackState : BaseState
        {
            private readonly int _attackHash = Animator.StringToHash("Attack");

            private readonly BaseWeapon _weapon;
            private readonly IAttackModel _attackModel;
            private readonly WeaponTimer _weaponTimer;
            
            private Unit Owner => StateMachine._owner;
            private ITarget Target => StateMachine._targetProvider.Target;
            private bool IsTargetInvalid => !Target.IsTargetValidAndAlive();
            
            public AttackState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
                _weapon = Owner.gameObject.RequireComponentInChildren<BaseWeapon>();
                _weaponTimer = new WeaponTimer(_attackModel.AttackInterval.Value);
            }

            public override void OnEnterState()
            {
                StateMachine._animationWrapper.PlayIdleSmooth();
                StateMachine._movementController.IsStopped = true;
                
                if (HasWeaponAnimationHandler)
                {
                    StateMachine._weaponAnimationHandler.OnFireEvent += Fire;
                }
            }

            public override void OnExitState()
            {
                if (HasWeaponAnimationHandler)
                {
                    StateMachine._weaponAnimationHandler.OnFireEvent -= Fire;
                }
            }

            public override void OnTick()
            {
                if (IsTargetInvalid)
                {
                    StateMachine.SetState(UnitState.Patrol);
                    return;
                }
                
                if (!IsTargetInAttackRange())
                {
                    StateMachine.SetState(UnitState.Chase);
                    return;
                }

                StateMachine._movementController.RotateTo(Target.Root.position);

                if (_weaponTimer.IsAttackReady)
                {
                    Attack();
                }
            }
            
            private void Attack()
            {
                if (!HasWeaponAnimationHandler)
                {
                    Fire();
                }

                StateMachine._animator.SetTrigger(_attackHash);
                _weaponTimer.OnAttack();
            }
            
            private void Fire()
            {
                if (IsTargetInvalid) return;
                _weapon.Fire(Target, null, DoDamage);
            }
            
            private bool IsTargetInAttackRange()
            {
                return Vector3.Distance(Target.Root.position, Owner.transform.position) < _attackModel.AttackDistance;
            }

            private void DoDamage(GameObject target)
            {
                var damageable = target.RequireComponent<IDamageable>();
                damageable.TakeDamage(_attackModel.AttackDamage);
                this.Logger().Trace($"Damage applied, target:= {target.name}");
            }

            private bool HasWeaponAnimationHandler => StateMachine._weaponAnimationHandler != null;
        }
    }
}