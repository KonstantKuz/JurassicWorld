using Dino.UI.Hud.Workbench;
using Feofun.Extension;
using Feofun.UI;
using UnityEngine;
using Zenject;

namespace Dino.Location.Workbench
{
    [RequireComponent(typeof(Workbench))]
    public class WorkbenchHudOwner : MonoBehaviour
    {
        [SerializeField] private Transform _hudPlace;
        [SerializeField] private WorkbenchHudPresenter _hudPrefab;

        private WorkbenchHudPresenter _hudPresenter;

        [Inject] private UIRoot _uiRoot;
        [Inject] private DiContainer _container;

        private void Awake()
        {
            CleanUp();
            _hudPresenter = _container.InstantiatePrefabForComponent<WorkbenchHudPresenter>(_hudPrefab);
            _hudPresenter.transform.SetParent(_uiRoot.HudContainer);
            _hudPresenter.Init(GetComponent<Workbench>());
        }
        private void Update()
        {
            if (_hudPresenter != null) {
                _hudPresenter.transform.position = _hudPlace.WorldToScreenPoint();
            }
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void CleanUp()
        {
            if (_hudPresenter == null) {
                return;
            }
            Destroy(_hudPresenter.gameObject);
            _hudPresenter = null;
        }
    }
}