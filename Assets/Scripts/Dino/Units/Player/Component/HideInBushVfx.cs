using System;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(AreaChangeDetector))]
    public class HideInBushVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _rendererRoot;
        [SerializeField] private float _transparencyInBush;

        private Renderer[] _renderers;
        private IDisposable _disposable;

        private void Awake()
        {
            _renderers = _rendererRoot.GetComponentsInChildren<Renderer>();            
            _disposable = GetComponent<AreaChangeDetector>().CurrentAreaType.Subscribe(OnAreaChanged);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void OnAreaChanged(AreaChangeDetector.AreaType areaType)
        {
            var alpha = areaType == AreaChangeDetector.AreaType.Grass ? _transparencyInBush : 1f;
            _renderers.ForEach(it => SetTransparency(it, alpha));
        }

        private static void SetTransparency(Renderer renderer, float alpha)
        {
        }
    }
}