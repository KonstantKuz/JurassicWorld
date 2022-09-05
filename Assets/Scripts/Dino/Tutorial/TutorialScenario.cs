using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public abstract class TutorialScenario : MonoBehaviour
    {
        public static Vector3 ARROW_OFFSET = Vector3.up;

        [SerializeField]
        private bool _isEnabled;
        [SerializeField]
        private TutorialScenarioId _scenarioId;
        
        [Inject]
        protected TutorialService TutorialService { get; }

        public bool IsEnabled => _isEnabled;
        public TutorialScenarioId ScenarioId => _scenarioId;
        protected ScenarioState State => TutorialService.GetScenarioState(ScenarioId);
        public bool IsCompleted => State.IsCompleted;

        public abstract void Init();
        
        protected bool IsStepCompleted(string stepName)
        {
            return State.CompletedSteps.Contains(stepName);
        }

        protected void CompleteStep(string stepName)
        {
            var state = State;
            state.CompletedSteps.Add(stepName);
            TutorialService.SetScenarioState(ScenarioId, state);
        }

        protected void CompleteScenario()
        {
            TutorialService.CompleteScenario(_scenarioId);
        }
    }
}