using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Inventory.Message;
using Dino.Location;
using Dino.Location.Level;
using Dino.Location.Level.Service;
using Dino.Location.Service;
using Dino.Session.Messages;
using Dino.Session.Service;
using Dino.Tutorial.Components;
using Dino.Tutorial.WaitConditions;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.Tutorial.Scenario
{
    public abstract class WorkbenchScenario : TutorialScenario
    {
        private const string WORKBENCH_ID = "Workbench";

        [SerializeField] private string _playAtLevelId;
        
        private List<IndicatedTutorialItem> _tutorialItems;
        private Coroutine _tutorialCoroutine;
        
        [Inject] private IMessenger _messenger;
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private SessionService _sessionService;
        [Inject] private NavigationService _navigationService;
        
        public override void Init()
        {
            _messenger.Subscribe<SessionEndMessage>(msg => Dispose());
            _messenger.Subscribe<SessionStartMessage>(OnSessionStart);
        }

        private void OnSessionStart(SessionStartMessage msg)
        {
            if (_sessionService.Session.LevelId != _playAtLevelId) return;

            CacheTutorialItems();
            _tutorialCoroutine = StartCoroutine(RunScenario());
        }

        private void CacheTutorialItems()
        {
            Assert.IsTrue(_world.Level != null, "Level is null. Init tutorial only on session start.");
            _tutorialItems = _world.Level.GetComponentsInChildren<IndicatedTutorialItem>().ToList();
        }

        public abstract IEnumerator RunScenario();

        protected List<Loot.Loot> GetTutorialLoots(string itemsId)
        {
            return _tutorialItems
                .Where(it => it != null && it.ItemId == itemsId)
                .Select(it => it.gameObject.RequireComponent<Loot.Loot>()).ToList();
        }
        
        protected IEnumerator WaitForLootCollected(List<Loot.Loot> loots)
        {
            return loots.Select(WaitForLootCollected).GetEnumerator();
        }
        
        private IEnumerator WaitForLootCollected(Loot.Loot loot)
        {
            var arrow = ArrowIndicator.SpawnAbove(_worldObjectFactory, loot.transform, ARROW_OFFSET);
            arrow.transform.SetParent(loot.transform);
            _navigationService.PointNavArrowAt(loot.transform);
            yield return new WaitForLootCollected(loot);
            _navigationService.HideNavArrow();
        }

        protected void PlayCameraLookAtWorkbench()
        {
            var workbench = _tutorialItems.First(it => it.ItemId == WORKBENCH_ID);
            TutorialService.PlayCameraLookAt(workbench.transform.position);
        }

        protected IEnumerator WaitForCraft()
        {
            var workbench = _tutorialItems.First(it => it.ItemId == WORKBENCH_ID);
            var indicator = ArrowIndicator.SpawnAbove(_worldObjectFactory, workbench.transform, ARROW_OFFSET);
            _navigationService.PointNavArrowAt(workbench.transform);
            yield return new WaitForMessage<ItemCraftedMessage>(_messenger);
            _navigationService.HideNavArrow();
            Destroy(indicator.gameObject);
        }

        protected void PlayCameraLookAtItems(List<Transform> items)
        {
            var itemsCenter = Vector3.zero;
            items.ForEach(it => itemsCenter += it.transform.position);
            itemsCenter /= items.Count;
            TutorialService.PlayCameraLookAt(itemsCenter);
        }

        private void Dispose()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStart);
            _messenger.Unsubscribe<SessionEndMessage>(msg => Dispose());

            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
                _tutorialCoroutine = null;
            }
        }
    }
}