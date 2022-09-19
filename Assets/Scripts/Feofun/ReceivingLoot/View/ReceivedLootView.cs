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
    public class ReceivedLootView : MonoBehaviour, IUiInitializable<ReceivedLootViewModel>
    {
        private const string SPEED_PARAM_NAME = "Speed"; 
        private const string ROTATING_PARAM_NAME = "Rotating";
        
        private readonly int _speedParam = Animator.StringToHash(SPEED_PARAM_NAME);
        private readonly int _rotatingParam = Animator.StringToHash(ROTATING_PARAM_NAME);

        [SerializeField] private Image _icon;
        [SerializeField] private Animator _animator;    
       
        private ReceivingTrajectoryTween _trajectory;
        private RectTransform _rectTransform;

        public void Init(ReceivedLootViewModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
            _rectTransform = gameObject.RequireComponent<RectTransform>();
            _trajectory = gameObject.RequireComponent<ReceivingTrajectoryTween>();
            StartCoroutine(PrepareDrop(model));
        }

        private IEnumerator PrepareDrop(ReceivedLootViewModel model)
        {
            _rectTransform.position = model.StartPosition;
            _rectTransform.localScale *= model.VfxConfig.ScaleFactorBeforeReceive;
            yield return _rectTransform.DOScale(Vector3.one, model.VfxConfig.TimeBeforeReceive).WaitForCompletion();
            _rectTransform.DOScale(Vector3.one * model.VfxConfig.FinalScaleFactor, model.Duration);
            StartRotateAnimation(model);
            _trajectory.Drop(ReceivedLootTrajectory.FromReceivedLootModel(model), () => { Destroy(gameObject); });
        }
        private void StartRotateAnimation(ReceivedLootViewModel viewModel)
        {
            _animator.SetFloat(_speedParam, Random.Range(0, viewModel.VfxConfig.RotationSpeedDispersion));
            _animator.SetBool(_rotatingParam, true);
        }
    }
}