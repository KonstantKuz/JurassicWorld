using System;
using System.Collections.Generic;
using System.Linq;
using Dino.Location;
using Dino.UI.Tutorial;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class TutorialService : MonoBehaviour, IWorldScope
    {
        [SerializeField]
        private List<TutorialScenario> _scenarios;

        [Inject]
        private TutorialRepository _repository;
        
        private TutorialState State => _repository.Get() ?? new TutorialState();

        public void OnWorldSetup()
        {
            _scenarios.ForEach(it => it.Init());
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

        public bool IsScenarioCompleted(TutorialScenarioId scenarioId)
        {
            return State.Scenarios.ContainsKey(scenarioId) && State.Scenarios[scenarioId].IsCompleted;
        }

        public void CompleteScenario(TutorialScenarioId scenarioId)
        {
            var state = State;
            state.Scenarios[scenarioId].IsCompleted = true;
            _repository.Set(state);
        }

        public void OnWorldCleanUp()
        {
        }
    }
}