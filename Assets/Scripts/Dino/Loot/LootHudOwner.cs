using Dino.UI.Hud.Loot;
using Dino.UI.Hud.Unit;
using UnityEngine;
using Zenject;

namespace Dino.Loot
{
    public class LootHudOwner : MonoBehaviour
    {
        [SerializeField] private Transform _hudPlace;
        [SerializeField] private LootHudPresenter _hudPrefab;

        private LootHudPresenter _hudPresenter;

        [Inject] private DiContainer _container;
        
        private void Awake()
        {
            CleanUp();
            _hudPresenter = _container.InstantiatePrefabForComponent<LootHudPresenter>(_hudPrefab);
            _hudPresenter.Init(_hudPlace);
            Hide();
        }
        
        public void ShowProgress(float progress)
        {
            _hudPresenter.gameObject.SetActive(true);
            _hudPresenter.ShowProgress(progress);
        }

        public void Hide()
        {
            _hudPresenter.gameObject.SetActive(false);
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