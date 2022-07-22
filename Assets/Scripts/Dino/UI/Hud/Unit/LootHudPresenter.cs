using Feofun.Extension;
using Feofun.UI;
using Feofun.UI.Components;
using UnityEngine;
using Zenject;

namespace Dino.UI.Hud.Unit
{
    public class LootHudPresenter : MonoBehaviour
    {
        [SerializeField] private ProgressBarView _progressBar;

        private Transform _hudPlace;
        
        [Inject] private UIRoot _uiRoot;

        public void Init(Transform place)
        {
            transform.SetParent(_uiRoot.HudContainer);
            _hudPlace = place;
        }

        public void ShowProgress(float progress)
        {
            _progressBar.Reset(progress);
            transform.position = _hudPlace.WorldToScreenPoint();
        }
    }
}