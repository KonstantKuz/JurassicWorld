using System.Collections.Generic;
using Feofun.Util.SerializableDictionary;
using UnityEngine;

namespace Dino.Units.Component.Animation
{
    public class AnimationSwitcher : MonoBehaviour
    {
        [SerializeField]
        private SerializableDictionary<string, AnimationClip> _animations;
        
        private Animator animator;
        private AnimatorOverrideController animatorOverrideController;
        
        public void Awake()
        {
            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        public void OverrideAnimation(string overriddenAnimationName, string animationId)
        {
            if (!_animations.ContainsKey(animationId)) {
                throw new KeyNotFoundException($"Animation:= {animationId} not found");
            }
            animatorOverrideController[overriddenAnimationName] = _animations[animationId];

        }

        public string GetAnimationName(string animationId)
        {
            if (!_animations.ContainsKey(animationId)) {
                throw new KeyNotFoundException($"Animation:= {animationId} not found");
            }
            return _animations[animationId].name;
        }
    }
}