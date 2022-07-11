using System;
using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Units.Component.Health;
using DinoWorldSurvival.Units.Enemy.Model;
using Feofun.Components;
using UniRx;
using UnityEngine;

namespace DinoWorldSurvival.Units.Enemy
{
    public class EnemySizeChanger : MonoBehaviour, IInitializable<IUnit>
    {
        private Health _health;
        private EnemyUnitModel _enemyModel;
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Init(IUnit unit)
        { 
            if (!(unit.Model is EnemyUnitModel enemyModel))
            {
                throw new ArgumentException($"Unit must be a enemy unit, gameObj:= {gameObject.name}");
            }
            _enemyModel = enemyModel;
            UpdateScale(_enemyModel.Level);
            _health = gameObject.RequireComponent<Health>();
            _health.CurrentValue.Subscribe(it => OnHealthChanged()).AddTo(_disposable);
        }
        
        private void OnHealthChanged()
        {
            var currentHealth = _health.CurrentValue.Value;
            var level = _enemyModel.CalculateLevelOfHealth(currentHealth);
            UpdateScale(level);
        }
        private void UpdateScale(int level)
        {
            var scale = _enemyModel.CalculateScale(level);
            transform.localScale = Vector3.one * scale;
        }
        private void OnDestroy()
        {
            _disposable?.Dispose();
            _health.OnDamageTaken -= OnHealthChanged;
        }
    }
}