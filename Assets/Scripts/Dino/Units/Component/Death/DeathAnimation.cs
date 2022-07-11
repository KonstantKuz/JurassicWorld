﻿using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Dino.Units.Component.Death
{
    public class DeathAnimation : MonoBehaviour, IUnitDeath
    { 
        private readonly int _deathHash = Animator.StringToHash("Death");
        
        [SerializeField]
        private float _disappearTime;       
        [SerializeField]
        private float _delayUntilDisappear;     
        [SerializeField]
        private float _offsetYDisappear;

        private Animator _animator;
        private Tweener _disappearTween;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
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
            _disappearTween = gameObject.transform.DOMoveY(transform.position.y - _offsetYDisappear, _disappearTime);
            yield return _disappearTween.WaitForCompletion(); 
            Destroy(gameObject);
        }

        private void EndAnimationIfStarted()
        {
            if (_disappearTween == null) return;
            _disappearTween.Kill(true);
            _disappearTween = null;
        }

        private void OnDisable()
        {
            EndAnimationIfStarted();
        }
    }
}
