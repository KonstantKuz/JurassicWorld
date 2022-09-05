using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dino.Location;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class TutorialService : MonoBehaviour, IWorldScope
    {
        [SerializeField] private float _cameraLookAtSpeed;
        [SerializeField] private float _cameraLookAtTime;
        private List<TutorialScenario> _scenarios;

        private List<TutorialScenario> Scenarios => _scenarios ??= GetComponentsInChildren<TutorialScenario>().ToList();
        
        [Inject] private TutorialRepository _repository;
        [Inject] private Joystick _joystick;
        [Inject] private World _world;
        
        private TutorialState State => _repository.Get() ?? new TutorialState();

        public void OnWorldSetup()
        {
            Scenarios.ForEach(it =>
            {
                if (it.IsEnabled && !it.IsCompleted)
                {
                    it.Init();
                }
            });
        }

        public ScenarioState GetScenarioState(TutorialScenarioId scenarioId)
        {
            if (!State.Scenarios.ContainsKey(scenarioId))
            {
                SetScenarioState(scenarioId, new ScenarioState());
            }
            
            return State.Scenarios[scenarioId];
        }

        public void SetScenarioState(TutorialScenarioId scenarioId, ScenarioState scenarioState)
        {
            var state = State;
            state.Scenarios[scenarioId] = scenarioState;
            _repository.Set(state);
        }

        public void CompleteScenario(TutorialScenarioId scenarioId)
        {
            var state = State;
            state.Scenarios[scenarioId].IsCompleted = true;
            _repository.Set(state);
        }

        public void SetInputEnabled(bool value)
        {
            _joystick.enabled = value;
        }

        public Tween PlayCameraLookAt(Transform point)
        {
            SetInputEnabled(false);
            _world.CameraController.IsFollowTarget = false;
            var lookAt = _world.CameraController.PlayLookAt(point, _cameraLookAtSpeed, _cameraLookAtTime);
            lookAt.onComplete = () =>
            {
                SetInputEnabled(true);
                _world.CameraController.IsFollowTarget = true;
            };
            return lookAt;
        }
        
        public void OnWorldCleanUp()
        {
        }
    }
}