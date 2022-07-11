﻿using System;
using DinoWorldSurvival.Location;
using Feofun.UI.Dialog;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using Zenject;

namespace DinoWorldSurvival.UI.Dialog.PauseDialog
{
    public class PauseDialog : BaseDialog
    {
        [Inject] private DialogManager _dialogManager;    
        [Inject] private World _world;      
        [Inject] private Joystick _joystick;
        
        private IDisposable _disposable;
        private readonly Subject<Unit> _closeEvent = new Subject<Unit>();

        public IObservable<Unit> CloseEvent => _closeEvent;

        private void OnEnable()
        {
            Dispose();
            _world.Pause();
            var uiBehaviour = _joystick.GetComponent<UIBehaviour>();
            _disposable = uiBehaviour.OnDragAsObservable().Merge(uiBehaviour.OnPointerClickAsObservable()).First().Subscribe(it => OnClick());
        }
        private void OnClick()
        {
            _dialogManager.Hide<PauseDialog>();
            _world.UnPause();
            _closeEvent.OnNext(Unit.Default);
            _closeEvent.OnCompleted();            
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}