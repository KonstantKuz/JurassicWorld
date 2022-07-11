using System;
using Dino.Session.Service;
using Feofun.UI.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World
{
    public class EnemiesLeftView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProLocalization _text;

        [Inject]
        private SessionService _sessionService;

        private CompositeDisposable _disposable;
        private void OnEnable()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _sessionService.Kills.Subscribe(OnKill).AddTo(_disposable);
        }

        private void OnKill(int killedCount)
        {
            var enemiesLeft = Math.Max(_sessionService.LevelConfig.KillCount - killedCount, 0);
            _text.SetTextFormatted(_text.LocalizationId, enemiesLeft);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}