using DG.Tweening;
using Logger.Extension;
using UnityEngine;

namespace Dino.Units.Component.Animation
{
    public class MoveAnimationWrapper
    {
        private const float SMOOTH_TRANSITION_TIME = 0.2f;
        private readonly int _verticalMotionHash = Animator.StringToHash("VerticalMotion");
        private readonly int _horizontalMotionHash = Animator.StringToHash("HorizontalMotion");
        private readonly int _motionAnimationHash = Animator.StringToHash("Motion");
        private readonly Animator _animator;

        public MoveAnimationWrapper(Animator animator)
        {
            _animator = animator;
        }

        public void PlayIdleSmooth()
        {
            _animator.CrossFade(_motionAnimationHash, SMOOTH_TRANSITION_TIME);
            AnimateMotionValues(0, 0);
        }

        public void PlayMoveForwardSmooth()
        {
            _animator.CrossFade(_motionAnimationHash, SMOOTH_TRANSITION_TIME);
            AnimateMotionValues(1, 0);
        }

        public void AnimateMotionValues(float vertical, float horizontal)
        {
            SmoothTransition(_verticalMotionHash, vertical, SMOOTH_TRANSITION_TIME);
            SmoothTransition(_horizontalMotionHash, horizontal, SMOOTH_TRANSITION_TIME);
        }

        private void SmoothTransition(int animationHash, float toValue, float time)
        {
            DOTween.To(() => TryGetValue(animationHash), value => TrySetValue(animationHash, value), toValue, time);
        }

        private float TryGetValue(int animationHash)
        {
            return _animator != null ? _animator.GetFloat(animationHash) : 0;
        }
        
        private void TrySetValue(int animationHash, float value)
        {
            if (_animator == null) return;
            _animator.SetFloat(animationHash, value);
        }
    }
}