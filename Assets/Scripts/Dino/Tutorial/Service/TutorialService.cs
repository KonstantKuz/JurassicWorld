using System.Collections.Generic;
using System.Linq;
using Dino.Location;
using Dino.Location.Level;
using Dino.Location.Service;
using Dino.Tutorial.Model;
using Dino.Tutorial.Scenario;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial.Service
{
    public class TutorialService : MonoBehaviour, IWorldScope
    {
        [SerializeField] private float _cameraLookAtSpeed;
        [SerializeField] private float _cameraLookAtTime;
        
        private List<TutorialScenario> _scenarios;
        private NavigationArrow _navigationArrow;
        
        [Inject] private TutorialRepository _repository;
        [Inject] private Joystick _joystick;
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private List<TutorialScenario> Scenarios => _scenarios ??= GetComponentsInChildren<TutorialScenario>().ToList();
        private TutorialState State => _repository.Get() ?? new TutorialState();
        private NavigationArrow NavigationArrow => _navigationArrow ??= InitNavigationArrow();

        private NavigationArrow InitNavigationArrow()
        {
            var arrow = NavigationArrow.Spawn(_worldObjectFactory);
            arrow.Parent = _world.RequirePlayer().transform;
            return arrow;
        }
        
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
            _joystick.OnPointerUp(null);
            _joystick.enabled = value;
        }

        public void PlayCameraLookAt(Vector3 point)
        {
            SetInputEnabled(false);
            _world.CameraController.PlayLookAt(point, _cameraLookAtSpeed, _cameraLookAtTime, () => SetInputEnabled(true));
        }

        public void NavigatePlayerTo([CanBeNull]Transform target)
        {
            NavigationArrow.gameObject.SetActive(target != null);
            NavigationArrow.Target = target;
        }

        public void OnWorldCleanUp()
        {
        }
    }
}