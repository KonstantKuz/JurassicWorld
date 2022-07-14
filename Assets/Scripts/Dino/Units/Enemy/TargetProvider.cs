using Dino.Units.Target;
using JetBrains.Annotations;
using UnityEngine;

namespace Dino.Units.Enemy
{
    public class TargetProvider : MonoBehaviour, ITargetProvider
    {
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

        private void ClearTarget()
        {
            Target = null;
        }
    }
}
