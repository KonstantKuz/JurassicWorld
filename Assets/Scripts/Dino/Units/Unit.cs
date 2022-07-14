using System;
using Dino.Core;
using Dino.Extension;
using Dino.Location.Model;
using Dino.Units.Component.Death;
using Dino.Units.Component.Health;
using Dino.Units.Model;
using Dino.Units.Player.Model;
using Dino.Units.Player.Movement;
using Dino.Units.Service;
using Dino.Units.Target;
using EasyButtons;
using Feofun.Components;
using Feofun.Modifiers;
using JetBrains.Annotations;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace Dino.Units
{
    public class Unit : WorldObject, IUnit
    {
        private IUpdatableComponent[] _updatables;
        private IDamageable _damageable;
        private IUnitDeath _death;
        private ITarget _selfTarget;
        private IUnitDeathEventReceiver[] _deathEventReceivers;
        private IUnitDeactivateEventReceiver[] _deactivateEventReceivers;
        private MovementController _movementController;
        private bool _isActive;
        private float _spawnTime;
        private Collider _collider;

        [Inject]
        private UnitService _unitService;
        [Inject]
        private UpdateManager _updateManager;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (!_isActive) {
                    _deactivateEventReceivers.ForEach(it => it.OnDeactivate());
                }
            }
        }

        public UnitType UnitType => _selfTarget.UnitType;
        public UnitType TargetUnitType => _selfTarget.UnitType.GetTargetUnitType();
        public ITarget SelfTarget => _selfTarget;
        public IUnitModel Model { get; private set; }
        public event Action<IUnit, DeathCause> OnDeath;
        public event Action<IUnit> OnUnitDestroyed;
        public MovementController MovementController => _movementController ??= GetComponent<MovementController>();

        public float LifeTime => Time.time - _spawnTime;
        [CanBeNull]
        public Health Health { get; private set; }
        public Bounds Bounds => _collider.bounds;

        public void Init(IUnitModel model)
        {
            Model = model;

            _updatables = GetComponentsInChildren<IUpdatableComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death = gameObject.RequireComponent<IUnitDeath>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();
            _deactivateEventReceivers = GetComponentsInChildren<IUnitDeactivateEventReceiver>();

            if (UnitType == UnitType.ENEMY)
            {
                _damageable.OnZeroHealth += DieOnZeroHealth;
            }

            IsActive = true;
            _spawnTime = Time.time;
            Health = GetComponent<Health>();
            _collider = GetComponent<CapsuleCollider>();

            foreach (var component in GetComponentsInChildren<IInitializable<IUnit>>()) {
                component.Init(this);
            }

            _unitService.Add(this);
            _updateManager.StartUpdate(UpdateComponents);
        }

        [Button]
        public void Kill(DeathCause deathCause)
        {
            _damageable.DamageEnabled = false;
            _damageable.OnZeroHealth -= DieOnZeroHealth;
            IsActive = false;
            _deathEventReceivers.ForEach(it => it.OnDeath(deathCause));
            _death.PlayDeath();
            OnDeath?.Invoke(this, deathCause);
            OnDeath = null;
        }

        private void DieOnZeroHealth()
        {
            Kill(DeathCause.Killed);
        }

        private void UpdateComponents()
        {
            if (!IsActive) {
                return;
            }
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }

        private void OnDestroy()
        {
            OnUnitDestroyed?.Invoke(this);
            OnUnitDestroyed = null;
            _unitService.Remove(this);
            _updateManager.StopUpdate(UpdateComponents);
        }

        public void AddModifier(IModifier modifier)
        {
            if (!(Model is PlayerUnitModel playerUnitModel)) {
                this.Logger().Error($"Unit model must be the PlayerUnitModel, current model:= {Model.GetType().Name}");
                return;
            }
            playerUnitModel.AddModifier(modifier);
        }
    }
}