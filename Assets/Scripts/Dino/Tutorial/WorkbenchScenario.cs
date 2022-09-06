using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    public abstract class WorkbenchScenario : TutorialScenario
    {
        private const string WORKBENCH_ID = "Workbench";

        [SerializeField] private string _playAtLevelId;
        
        private List<IndicatedTutorialItem> _tutorialItems;
        
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

        public abstract IEnumerator RunScenario();

        protected List<Loot.Loot> GetTutorialLoots(string itemsId)
        {
            return _tutorialItems
                .Where(it => it.ItemId == itemsId)
                .Select(it => it.gameObject.RequireComponent<Loot.Loot>()).ToList();
        }
        
        protected IEnumerator WaitForLootCollected(List<Loot.Loot> loots)
        {
            loots.ForEach(it =>
            {
                var arrow = ArrowIndicator.SpawnAbove(_worldObjectFactory, it.transform, ARROW_OFFSET);
                arrow.transform.SetParent(it.transform);
            });
            yield return new WaitForLootCollected(loots);
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
            
            yield return new WaitForMessage<ItemCraftedMessage>(_messenger);
            
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
        }
    }
}