using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(AreaChangeDetector))]
    public class HideInBushVfx : MonoBehaviour
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Material _hiddenMaterial;

        private readonly Dictionary<Renderer, Material> _initialMaterials = new Dictionary<Renderer, Material>();
        private IDisposable _disposable;

        private void Awake()
        {
            _disposable = GetComponent<AreaChangeDetector>().CurrentAreaType.Subscribe(OnAreaChanged);
            foreach (var renderer in _root.GetComponentsInChildren<Renderer>())
            {
                _initialMaterials[renderer] = renderer.material;
            }
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
            foreach (var renderer in _initialMaterials.Keys)
            {
                renderer.material = isVisible ? _initialMaterials[renderer] : _hiddenMaterial;
            }
        }
    }
}