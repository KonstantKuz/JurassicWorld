using Dino.Units.Component;
using Dino.Units.Hud;
using Feofun.UI;
using Feofun.UI.Components;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace Dino.UI.Hud.Unit
{
    public class HudPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private TextMeshProUGUI _levelStatText;
        
        private Transform _hudPlace;
        
        [Inject] private UIRoot _uiRoot;

        public void Init(HudOwner hudOwner, Transform hudPlace)
        {
            transform.SetParent(_uiRoot.HudContainer);
            _hudPlace = hudPlace;
            InitHealthBar(hudOwner.HealthBarOwner);
            InitLevelStat(hudOwner.LevelStatOwner);
        }
        private void InitHealthBar(IHealthBarOwner healthBarOwner)
        {
            var model = new HealthBarModel(healthBarOwner);
            _healthBarView.Init(model);
        }
        private void InitLevelStat([CanBeNull] ILevelStatOwner levelStatOwner)
        {
            if (_levelStatText == null || levelStatOwner == null)
            {
                return;
            }
            _levelStatText.SetText(levelStatOwner.Level.ToString());
        }
        private void Update()
        {
            if (_hudPlace == null) {
                return;
            }

            var worldToScreenPoint = UnityEngine.Camera.main.WorldToScreenPoint(_hudPlace.position);
            transform.position = worldToScreenPoint;
        }
    }
}
