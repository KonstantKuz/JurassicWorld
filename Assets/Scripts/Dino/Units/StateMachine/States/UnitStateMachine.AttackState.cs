using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Model;
using Dino.Units.Player.Attack;
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

            private BaseWeapon _weapon;
            private IAttackModel _attackModel;
            private IWeaponTimerManager _weaponTimer;
            
            private Unit Owner => StateMachine._owner;

            public AttackState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                _attackModel = Owner.Model.AttackModel;
                _weapon = Owner.gameObject.RequireComponentInChildren<BaseWeapon>();
                _weaponTimer = _weapon.GetComponent<IWeaponTimerManager>() ?? Owner.gameObject.GetComponent<IWeaponTimerManager>();
                
                _weaponTimer.Subscribe(Owner.ObjectId, _attackModel, Attack);
            }

            public override void OnEnterState()
            {
                StateMachine._agent.isStopped = true;
                
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
                if (StateMachine.IsTargetInvalid || !IsTargetInRange())
                {
                    StateMachine.SetState(new IdleState(StateMachine));
                    return;
                }
                
                RotateTo(StateMachine.Target.Root.position);
            }
            
            private void Attack()
            {
                if (!HasWeaponAnimationHandler)
                {
                    Fire();
                }
                StateMachine._animator.SetTrigger(_attackHash);                
            }
            
            private void Fire()
            {
                if (StateMachine.IsTargetInvalid) return;
                _weapon.Fire(StateMachine.Target, null, DoDamage);
            }
            
            private void RotateTo(Vector3 targetPos)
            {
                var transform = StateMachine.transform;
                var lookAtDirection = (targetPos - transform.position).XZ().normalized;
                var lookAt = Quaternion.LookRotation(lookAtDirection, transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookAt, Time.deltaTime * StateMachine._rotationSpeed);
            }

            private bool IsTargetInRange()
            {
                return Vector3.Distance(StateMachine.Target.Root.position, Owner.transform.position) < _attackModel.AttackDistance;
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