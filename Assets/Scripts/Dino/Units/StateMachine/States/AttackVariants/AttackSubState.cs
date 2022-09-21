using System;
using Dino.Units.Component;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Units.Enemy.Model.EnemyAttack;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        public abstract class AttackSubState
        {
            protected readonly int AttackHash = Animator.StringToHash("Attack");

            protected readonly UnitStateMachine StateMachine;
            protected readonly EnemyAttackModel AttackModel;
            protected readonly Action<GameObject> HitCallback;

            [CanBeNull]
            protected IFieldOfViewRenderer _fieldOfViewRenderer;
            
            protected bool HasWeaponAnimationHandler => StateMachine._weaponAnimationHandler != null;
            protected Unit Owner => StateMachine._owner;
            [CanBeNull]
            protected ITarget Target => StateMachine._targetProvider.Target;
            protected Vector3 TargetPosition => Target.Root.position;
            protected bool IsTargetInvalid => !Target.IsTargetValidAndAlive();
            public bool IsTargetInAttackRange => Vector3.Distance(Owner.transform.position, Target.Root.position) < AttackModel.AttackDistance;
            protected bool IsTargetBlocked =>
                Physics.Linecast(Owner.SelfTarget.Center.position, Target.Center.position, StateMachine._layerMaskProvider.ObstacleMask);

            protected AttackSubState(UnitStateMachine stateMachine, EnemyAttackModel attackModel, Action<GameObject> hitCallback)
            {
                StateMachine = stateMachine;
                AttackModel = attackModel;
                HitCallback = hitCallback;
                
                _fieldOfViewRenderer = Owner.gameObject.GetComponentInChildren<IFieldOfViewRenderer>();
            }
            
            public abstract void OnEnterState();
            public abstract void OnTick();
            public abstract void OnExitState();

            protected virtual void TryHit(Collider collider)
            {
                if(!CanDamage(collider)) return;
                HitCallback?.Invoke(collider.gameObject);
            }

            private bool CanDamage(Collider collider)
            {
                return collider.TryGetComponent(out IDamageable damageable) &&
                       StateMachine._layerMaskProvider.DamageMask.Contains(collider.gameObject.layer);
            }

            protected bool IsRequiredPatrolState()
            {
                if (!IsTargetInvalid) return false;
                StateMachine.SetState(UnitState.Patrol);
                return true;
            }

            protected virtual bool IsRequiredChaseState()
            {
                if (IsTargetInAttackRange && !IsTargetBlocked) return false;
                StateMachine.SetState(UnitState.Chase);
                return true;
            }
        }
    }
}