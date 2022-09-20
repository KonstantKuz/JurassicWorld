using System;
using Dino.Extension;
using Dino.Units.Enemy.Model.EnemyAttack;
using Dino.Weapon;
using Dino.Weapon.Components;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        public class RegularAttack : AttackSubState
        {
            private readonly BaseWeapon _weapon;
            private readonly WeaponTimer _weaponTimer;

            public RegularAttack(UnitStateMachine stateMachine, EnemyAttackModel attackModel, Action<GameObject> hitCallback) : base(stateMachine, attackModel, hitCallback)
            {
                _weapon = Owner.gameObject.RequireComponentInChildren<BaseWeapon>();
                _weaponTimer = new WeaponTimer(AttackModel.AttackInterval);
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
                
                if (!IsTargetInAttackRange)
                {
                    StateMachine.SetState(UnitState.Chase);
                    return;
                }

                StateMachine._movementController.RotateTo(TargetPosition, 0f);

                if (_weaponTimer.IsAttackReady.Value)
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

                StateMachine._animator.SetTrigger(AttackHash);
                _weaponTimer.OnAttack();
            }
            
            private void Fire()
            {
                if (IsTargetInvalid) return;
                _weapon.Fire(Target, null, HitCallback);
            }
        }
    }
}