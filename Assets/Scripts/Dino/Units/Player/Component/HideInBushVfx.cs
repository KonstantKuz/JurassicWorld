using System;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(AreaChangeDetector))]
    public class HideInBushVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _visibleRoot;
        [SerializeField] private GameObject _hiddenRoot;

        private Renderer[] _renderers;
        private IDisposable _disposable;

        private void Awake()
        {
            _disposable = GetComponent<AreaChangeDetector>().CurrentAreaType.Subscribe(OnAreaChanged);
            SetVisible(true);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void OnAreaChanged(AreaChangeDetector.AreaType areaType)
        {
            SetVisible(areaType != AreaChangeDetector.AreaType.Grass);
        }

        private void SetVisible(bool isVisible)
        {
            _visibleRoot.SetActive(isVisible);
            _hiddenRoot.SetActive(!isVisible);
        }
    }
}