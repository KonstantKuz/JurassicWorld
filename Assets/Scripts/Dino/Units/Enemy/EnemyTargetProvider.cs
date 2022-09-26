using Dino.Extension;
using Dino.Units.Component.Target;
using Dino.Units.Component.TargetSearcher;
using Feofun.Components;
using Feofun.Extension;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.Enemy
{
    public class EnemyTargetProvider : MonoBehaviour, ITargetProvider, IInitializable<Unit>, IUpdatableComponent
    {
        private ITargetSearcher _targetSearcher;
        [CanBeNull] private ITarget _target;

        public ITarget Target
        {
            get => _target;
            set
            {
                if (_target == value) return;
                if (_target != null)
                {
                    _target.OnTargetInvalid -= ClearTarget;
                }
                _target = value;
                if (_target != null)
                {
                    _target.OnTargetInvalid += ClearTarget;
                }
            }
        }

        public void Init(Unit owner)
        {
            _targetSearcher = owner.GameObject.RequireComponent<ITargetSearcher>();
        }

        public void OnTick()
        {
            if (Target == null || !Target.IsTargetValidAndAlive())
            {
                Target = _targetSearcher.Find();
            }
        }


        private void ClearTarget()
        {
            Target = null;
        }
    }
}
