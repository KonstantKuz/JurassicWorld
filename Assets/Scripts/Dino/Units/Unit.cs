using System;
using Dino.Core;
using Dino.Extension;
using Dino.Location.Model;
using Dino.Units.Component.Death;
using Dino.Units.Component.Health;
using Dino.Units.Component.Target;
using Dino.Units.Model;
using Dino.Units.Player.Movement;
using Dino.Units.Service;
using EasyButtons;
using Feofun.Components;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
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

        [CanBeNull]
        public Health Health { get; private set; }

        private void Awake()
        {
            _updatables = GetComponentsInChildren<IUpdatableComponent>();
            _damageable = gameObject.RequireComponent<IDamageable>();
            _death = gameObject.RequireComponent<IUnitDeath>();
            _selfTarget = gameObject.RequireComponent<ITarget>();
            _deathEventReceivers = GetComponentsInChildren<IUnitDeathEventReceiver>();
            _deactivateEventReceivers = GetComponentsInChildren<IUnitDeactivateEventReceiver>();
            Health = GetComponent<Health>();
        }

        public void Init(IUnitModel model)
        {
            Model = model;
            _damageable.OnZeroHealth += DieOnZeroHealth;
            IsActive = true;
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
    }
}