using System;
using Dino.Units.Component.Target;
using UniRx;
using UnityEngine;

namespace Dino.Units.Player.Component
{
    [RequireComponent(typeof(AreaChangeDetector))]
    [RequireComponent(typeof(UnitTarget))]
    public class HideTargetInGrass : MonoBehaviour
    {
        private UnitTarget _unitTarget;
        private IDisposable _disposable;
        
        private void Awake()
        {
            _unitTarget = GetComponent<UnitTarget>();
            _disposable = GetComponent<AreaChangeDetector>().CurrentAreaType.Subscribe(OnAreaChanged);
        }

        private void OnDestroy()
        {
            _disposable.Dispose();
        }

        private void OnAreaChanged(AreaChangeDetector.AreaType areaType)
        {
            _unitTarget.Hidden = areaType == AreaChangeDetector.AreaType.Grass;
        }
    }
}