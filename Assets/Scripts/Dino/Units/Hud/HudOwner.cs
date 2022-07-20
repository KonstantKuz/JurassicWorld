using Dino.UI.Hud.Unit;
using Dino.Units.Component;
using Feofun.Components;
using UniRx;
using UnityEngine;
using Zenject;
namespace Dino.Units.Hud
{
    public class HudOwner : MonoBehaviour, IInitializable<Unit>
    {
        [SerializeField] private HudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;

        private HudPresenter _hudPresenter;
        private IHealthBarOwner _healthBarOwner;

        [Inject]
        private DiContainer _container;

        public IHealthBarOwner HealthBarOwner => _healthBarOwner ??= GetComponent<IHealthBarOwner>();

        public void Init(Unit unit)
        {
            CleanUp();
            _hudPresenter = _container.InstantiatePrefabForComponent<HudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
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