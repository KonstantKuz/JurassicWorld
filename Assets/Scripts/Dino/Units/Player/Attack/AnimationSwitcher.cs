using System.Collections.Generic;
using UnityEngine;
using Feofun.Util.SerializableDictionary;

namespace Dino.Units.Player.Attack
{
    public class AnimationSwitcher : MonoBehaviour
    {
        [SerializeField]
        public SerializableDictionary<string, AnimationClip> Animations;
        
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
            if (!Animations.ContainsKey(animationId)) {
                throw new KeyNotFoundException($"Animation:= {animationId} not found");
            }
            animatorOverrideController[overriddenAnimationName] = Animations[animationId];

        }

        public string GetAnimationName(string animationId)
        {
            if (!Animations.ContainsKey(animationId)) {
                throw new KeyNotFoundException($"Animation:= {animationId} not found");
            }
            return Animations[animationId].name;
        }
    }
}