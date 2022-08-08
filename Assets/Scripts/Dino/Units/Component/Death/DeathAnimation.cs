using System;
using System.Collections;
using DG.Tweening;
using Dino.Extension;
using Feofun.Extension;
using UnityEngine;

namespace Dino.Units.Component.Death
{
    public enum DeathAnimationType
    {
        FallDown,
        ScaleDown,
        Disappear,
    }
    
    public class DeathAnimation : MonoBehaviour, IUnitDeath
    { 
        private readonly int _deathHash = Animator.StringToHash("Death");
        [SerializeField]
        private DeathAnimationType _animationType;
        [SerializeField]
        private float _disappearTime;       
        [SerializeField]
        private float _delayUntilDisappear;     
        [SerializeField]
        private float _offsetYDisappear;

        private Animator _animator;
        private Tweener _deathTween;
        private Renderer _renderer;

        private void Awake()
        {
            _animator = gameObject.RequireComponentInChildren<Animator>();
            _renderer = gameObject.RequireComponentInChildren<Renderer>();
        }

        public void PlayDeath()
        {
            StartCoroutine(Disappear());
        }
        
        private IEnumerator Disappear()
        {
            EndAnimationIfStarted();
            
            _animator.SetTrigger(_deathHash);            
            yield return new WaitForSeconds(_delayUntilDisappear);
            _deathTween = DeathTween();
            yield return _deathTween.WaitForCompletion(); 
            Destroy(gameObject);
        }

        private void EndAnimationIfStarted()
        {
            if (_deathTween == null) return;
            _deathTween.Kill(true);
            _deathTween = null;
        }

        private Tweener DeathTween()
        {
            switch (_animationType)
            {
                case DeathAnimationType.FallDown:
                    return gameObject.transform.DOMoveY(transform.position.y - _offsetYDisappear, _disappearTime);
                case DeathAnimationType.ScaleDown:
                    return gameObject.transform.DOScale(0, _disappearTime);
                case DeathAnimationType.Disappear:
                    return DisappearTween();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Tweener DisappearTween()
        {
            _renderer.material.ToTransparent();
            return _renderer.material.DOFade(0, _disappearTime);
        }
        
        private void OnDisable()
        {
            EndAnimationIfStarted();
        }
    }
}
