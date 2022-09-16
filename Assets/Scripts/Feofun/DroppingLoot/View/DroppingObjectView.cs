using System.Collections;
using DG.Tweening;
using Feofun.DroppingLoot.Model;
using Feofun.DroppingLoot.Tween;
using Feofun.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Feofun.DroppingLoot.View
{
    public class DroppingObjectView : MonoBehaviour, IUiInitializable<DroppingObjectViewModel>
    {
        private const string SPEED_PARAM_NAME = "Speed"; 
        private const string ROTATING_PARAM_NAME = "Rotating";
        
        private readonly int _speedParam = Animator.StringToHash(SPEED_PARAM_NAME);
        private readonly int _rotatingParam = Animator.StringToHash(ROTATING_PARAM_NAME);

        [SerializeField] private Image _icon;
        [SerializeField] private Animator _animator;    
       
        private DroppingTrajectoryTween _trajectory;
        private RectTransform _rectTransform;

        public void Init(DroppingObjectViewModel model)
        {
            _icon.sprite = Resources.Load<Sprite>(model.Icon);
            _rectTransform = GetComponent<RectTransform>();
            _trajectory = GetComponent<DroppingTrajectoryTween>();
            StartCoroutine(PrepareDrop(model));
        }

        private IEnumerator PrepareDrop(DroppingObjectViewModel model)
        {
            _rectTransform.position = model.StartPosition;
            _rectTransform.localScale *= model.Config.ScaleFactorBeforeDrop;
            yield return _rectTransform.DOScale(Vector3.one, model.Config.TimeBeforeDrop).WaitForCompletion();
            _rectTransform.DOScale(Vector3.one * model.Config.FinalScaleFactor, model.Duration);
            StartRotateAnimation(model);
            _trajectory.Drop(DroppingObjectTrajectory.FromDroppingObject(model), () => { Destroy(gameObject); });
        }
        private void StartRotateAnimation(DroppingObjectViewModel viewModel)
        {
            _animator.SetFloat(_speedParam, Random.Range(0, viewModel.Config.RotationSpeedDispersion));
            _animator.SetBool(_rotatingParam, true);
        }
    }
}