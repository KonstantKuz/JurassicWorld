using System.Collections;
using DG.Tweening;
using Feofun.Extension;
using Feofun.ReceivingLoot.Model;
using Feofun.ReceivingLoot.Tween;
using Feofun.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.ReceivingLoot.View
{
    public class FlyingIconView : MonoBehaviour, IUiInitializable<FlyingIconViewModel>
    {
        private const string SPEED_PARAM_NAME = "Speed"; 
        private const string ROTATING_PARAM_NAME = "Rotating";
        
        private readonly int _speedParam = Animator.StringToHash(SPEED_PARAM_NAME);
        private readonly int _rotatingParam = Animator.StringToHash(ROTATING_PARAM_NAME);

        [SerializeField] private Image _icon;
        [SerializeField] private Animator _animator;    

        private RectTransform _rectTransform;

        public void Init(FlyingIconViewModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
            _rectTransform = gameObject.RequireComponent<RectTransform>();
            StartCoroutine(PrepareDrop(model));
        }

        private IEnumerator PrepareDrop(FlyingIconViewModel model)
        {
            _rectTransform.position = model.StartPosition;
            _rectTransform.localScale *= model.VfxConfig.ScaleFactorBeforeReceive;
            yield return _rectTransform.DOScale(Vector3.one, model.VfxConfig.TimeBeforeReceive).WaitForCompletion();
            _rectTransform.DOScale(Vector3.one * model.VfxConfig.FinalScaleFactor, model.Duration);
            StartRotateAnimation(model);
            yield return FlyingIconTrajectoryTween.Play(FlyingIconTrajectory.FromFlyingIconViewModel(model), _rectTransform).WaitForCompletion();
            Destroy(gameObject);
        }
        private void StartRotateAnimation(FlyingIconViewModel viewModel)
        {
            _animator.SetFloat(_speedParam, Random.Range(0, viewModel.VfxConfig.RotationSpeedDispersion));
            _animator.SetBool(_rotatingParam, true);
        }
    }
}