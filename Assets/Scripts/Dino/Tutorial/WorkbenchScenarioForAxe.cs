using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Inventory.Message;
using Dino.Location;
using Dino.Location.Level;
using Dino.Location.Service;
using Dino.Session.Messages;
using Dino.Session.Service;
using Dino.Tutorial.WaitConditions;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace Dino.Tutorial
{
    public class WorkbenchScenarioForAxe : TutorialScenario
    {
        private static Vector3 ARROW_OFFSET = Vector3.up;
        
        private const string STONE_ID = "Stone";
        private const string WORKBENCH_ID = "Workbench";
        
        [SerializeField] private string _playAtLevelId;
        
        private List<IndicatedTutorialItem> _tutorialItems;
        private int _currentStep;
        
        [Inject] private IMessenger _messenger;
        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private SessionService _sessionService;
        
        public override void Init()
        {
            _messenger.Subscribe<SessionEndMessage>(msg => Dispose());
            _messenger.Subscribe<SessionStartMessage>(OnSessionStart);
        }

        private void OnSessionStart(SessionStartMessage msg)
        {
            if (_sessionService.Session.LevelId != _playAtLevelId) return;

            CacheTutorialItems();
            StartCoroutine(RunScenario());
        }

        private void CacheTutorialItems()
        {
            _tutorialItems = _world.Level.GetComponentsInChildren<IndicatedTutorialItem>().ToList();
        }

        private IEnumerator RunScenario()
        {
            yield return WaitForFirstItemsCollected();
            yield return WaitForAxeCrafted();
            CompleteScenario();
        }

        private IEnumerator WaitForFirstItemsCollected()
        {
            var firstPointAtItems = _tutorialItems
                .Where(it => it.ItemId == STONE_ID)
                .Select(it => it.gameObject.RequireComponent<Loot.Loot>()).ToList();

            firstPointAtItems.ForEach(it => ArrowIndicator.SpawnAbove(_worldObjectFactory, it.transform, ARROW_OFFSET, true));
            
            yield return new WaitForLootCollected(firstPointAtItems);
        }

        private IEnumerator WaitForAxeCrafted()
        {
            var workbench = _tutorialItems.First(it => it.ItemId == WORKBENCH_ID);
            var indicator = ArrowIndicator.SpawnAbove(_worldObjectFactory, workbench.transform, ARROW_OFFSET);
            
            yield return new WaitForMessage<ItemCraftedMessage>(_messenger);
            
            Destroy(indicator.gameObject);
        }

        private void Dispose()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStart);
            _messenger.Unsubscribe<SessionEndMessage>(msg => Dispose());
        }
    }
}