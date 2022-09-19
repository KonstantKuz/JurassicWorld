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
        private CompositeDisposable _disposable;
        
        [Inject] private LootFactory _lootFactory;
        [Inject] private ConstantsConfig _constantsConfig;
        
        public void OnWorldSetup()
        {
            Dispose();
            _disposable = new CompositeDisposable();
        }

        public void AddToRespawn(string lootId, ReceivedItem receivedItem, Vector3 position)
        {
            Observable.Timer(TimeSpan.FromSeconds(_constantsConfig.ItemRespawnTime))
                .Subscribe(it => { RespawnLoot(lootId, receivedItem, position); })
                .AddTo(_disposable);
        }

        private void RespawnLoot(string lootId, ReceivedItem receivedItem, Vector3 position)
        {
            var respawnedLoot = _lootFactory.CreateLoot(lootId, receivedItem);
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