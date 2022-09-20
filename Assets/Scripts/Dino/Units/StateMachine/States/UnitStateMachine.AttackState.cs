using System;
using Dino.Extension;
using Dino.Units.Component.Health;
using Dino.Units.Enemy.Model.EnemyAttack;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.StateMachine
{
    public partial class UnitStateMachine
    {
        private class AttackState : BaseState
        {
            private readonly EnemyAttackModel _attackModel;
            private AttackSubState _currentAttack;
            
            public AttackState(UnitStateMachine stateMachine) : base(stateMachine)
            {
                var enemyModel = StateMachine._owner.RequireEnemyModel();
                _attackModel = enemyModel.AttackModel;
                _currentAttack = BuildAttack(_attackModel.AttackVariant);
            }

            public override void OnEnterState()
            {
                _currentAttack.OnEnterState();
            }

            public override void OnExitState()
            {
                _currentAttack.OnExitState();
            }

            public override void OnTick()
            {
                _currentAttack.OnTick();
            }
            
            private void DoDamage(GameObject target)
            {
                var damageable = target.RequireComponent<IDamageable>();
                var damageParams = new HitParams
                {
                    Damage = _attackModel.AttackDamage,
                    AttackerPosition = StateMachine._owner.SelfTarget.Root.position
                };
                damageable.TakeDamage(damageParams);
                this.Logger().Trace($"Damage applied, target:= {target.name}");
            }

            private AttackSubState BuildAttack(AttackVariant attackVariant)
            {
                return attackVariant switch
                {
                    AttackVariant.Regular => new RegularAttack(StateMachine, _attackModel, DoDamage),
                    AttackVariant.Bulldozing => new BulldozingAttack(StateMachine, _attackModel, DoDamage),
                    AttackVariant.Jumping => new JumpingAttack(StateMachine, _attackModel, DoDamage),
                    _ => throw new ArgumentOutOfRangeException(nameof(attackVariant), attackVariant, null)
                };
            }
        }
    }
}