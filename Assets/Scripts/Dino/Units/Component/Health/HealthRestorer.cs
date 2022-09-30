using System;
using Dino.Units.Player.Model;
using Feofun.Components;
using UnityEngine;

namespace Dino.Units.Component.Health
{
    [RequireComponent(typeof(UnitWithHealth))]
    public class HealthRestorer : MonoBehaviour, IInitializable<Unit>, IUpdatableComponent
    {
        private UnitWithHealth _unitWithHealth;
        private HealthRestoreModel _healthRestoreModel;

        private float _recoveryPeriodTimeLeft;
        private float _timeLeftBeforeRecover;

        public void Init(Unit unit)
        {
            var healthModel = unit.Model.HealthModel as PlayerHealthModel;
            if (healthModel == null) {
                throw new InvalidCastException($"The HealthModel must be of type:= {nameof(PlayerHealthModel)}");
            }
            _healthRestoreModel = healthModel.HealthRestoreModel;

            ResetTimeBeforeRecover();
            ResetRecoveryPeriod();

            _unitWithHealth = GetComponent<UnitWithHealth>();
            _unitWithHealth.OnDamageTaken += ResetTimeBeforeRecover;
        }

        private void ResetRecoveryPeriod() => _recoveryPeriodTimeLeft = _healthRestoreModel.RecoveryPeriod;

        private void ResetTimeBeforeRecover() => _timeLeftBeforeRecover = _healthRestoreModel.TimeoutBeforeRecover;

        private void ResetTimeBeforeRecover(HitParams obj) => ResetTimeBeforeRecover();

        public void OnTick()
        {
            _timeLeftBeforeRecover -= Time.deltaTime;
            if (_timeLeftBeforeRecover > 0) return;
            RecoverPerPeriod();
        }

        private void RecoverPerPeriod()
        {
            _recoveryPeriodTimeLeft -= Time.deltaTime;
            if (_recoveryPeriodTimeLeft > 0) return;
            _unitWithHealth.Add(_healthRestoreModel.RecoveryValue);
            ResetRecoveryPeriod();
        }

        private void OnDisable() => _unitWithHealth.OnDamageTaken -= ResetTimeBeforeRecover;
    }
}