using System;
using System.Collections.Generic;
using Dino.Config;
using Dino.Location;
using Dino.Location.Workbench;
using Dino.Loot.Messages;
using Feofun.Extension;
using SuperMaxim.Messaging;
using UniRx;
using UnityEngine;
using Zenject;

namespace Dino.Loot.Service
{
    public class LootRespawnService : IWorldScope
    {
        private List<Loot> _levelLoots;
        private List<ActionTimer> _respawnTimers;
        private CompositeDisposable _disposable;
        
        [Inject] private IMessenger _messenger;
        [Inject] private LootService _lootService;
        [Inject] private ConstantsConfig _constantsConfig;
        
        public void OnWorldSetup()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _messenger.SubscribeWithDisposable<LootCollectedMessage>(RespawnLootWithDelay).AddTo(_disposable);
        }

        private void RespawnLootWithDelay(LootCollectedMessage msg)
        {
            Observable.Timer(TimeSpan.FromSeconds(_constantsConfig.ItemRespawnTime))
                .Subscribe(it => { RespawnLoot(msg.Id, msg.ReceivedItem, msg.Position); })
                .AddTo(_disposable);
        }

        private void RespawnLoot(string id, ReceivedItem receivedItem, Vector3 position)
        {
            var lootPrefab = _lootService.FindLootPrefab(id);
            if(lootPrefab == null) return;
            
            var respawnedLoot = _lootService.SpawnLoot(id, receivedItem);
            respawnedLoot.transform.position = position;
        }

        public void OnWorldCleanUp()
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