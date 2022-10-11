using DG.Tweening;
using Feofun.Extension;
using Logger.Extension;
using UniRx;
using UniRx.Triggers;
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
        private readonly CompositeDisposable _disposable;

        public MoveAnimationWrapper(Animator animator)
        {
            _animator = animator;
            _disposable = new CompositeDisposable();
            _animator.gameObject.OnDestroyAsObservable().Subscribe(it => Dispose()).AddTo(_disposable);
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
            var valuesTween = DOTween.To(() => _animator.GetFloat(animationHash), value => _animator.SetFloat(animationHash, value), toValue, time);
            valuesTween.ToDisposable().AddTo(_disposable);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}