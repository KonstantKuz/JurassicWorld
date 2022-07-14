using UnityEngine;
using Feofun.Util.SerializableDictionary;

namespace Dino.Units.Player.Attack
{
    public class AttackAnimationSwitcher : MonoBehaviour
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

        public void ChangeAnimation()
        {
            
        }

        public void Update()
        {
            if (Input.GetButtonDown("NextWeapon"))
            {
                weaponIndex = (weaponIndex + 1) % weaponAnimationClip.Length;
                animatorOverrideController["shot"] = weaponAnimationClip[weaponIndex];
            }
        }
    }
}