using System.Collections;
using Dino.Session.Model;
using Dino.UI.Screen.Debriefing.Model;
using Feofun.UI.Components;
using UnityEngine;

namespace Dino.UI.Screen.Debriefing
{
    public class SessionResultPanel : MonoBehaviour
    {
        [SerializeField] private float _animateValuesDelay;
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private AnimatedIntView _killCountText;
        [SerializeField] private AnimatedIntView _coinsCountText;

        private ResultPanelModel _model;
        private Coroutine _showStatisticsCoroutine;

        public void Init(ResultPanelModel model)
        {
            _model = model;
            _winPanel.SetActive(model.SessionResult == SessionResult.Win);     
            _losePanel.SetActive(model.SessionResult == SessionResult.Lose);
            ResetStatistics();
            StopStatisticAnimation();
            _showStatisticsCoroutine = StartCoroutine(ShowStatisticWithDelay());
        }

        private void ResetStatistics()
        {
            _killCountText.Reset(0);
            _coinsCountText.Reset(0);
        }

        private IEnumerator ShowStatisticWithDelay()
        {
            yield return new WaitForSeconds(_animateValuesDelay);
            InitStatistics(_model.KillCount, _model.CoinsCount);
            _showStatisticsCoroutine = null;
        }

        private void InitStatistics(int killCount, int coinsCount)
        {
            _killCountText.SetData(killCount);
            _coinsCountText.SetData(coinsCount);
        }

        private void OnDisable()
        {
            StopStatisticAnimation();
        }

        private void StopStatisticAnimation()
        {
            if (_showStatisticsCoroutine == null) return;
            StopCoroutine(_showStatisticsCoroutine);
            _showStatisticsCoroutine = null;
        }
    }
}
