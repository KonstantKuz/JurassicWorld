using System;
using System.Linq;
using Dino.Location;
using Dino.Location.Service;
using Dino.Loot.Config;
using Dino.Units;
using Dino.Units.Service;
using Feofun.Config;
using Logger.Extension;
using Zenject;
using Random = UnityEngine.Random;

namespace Dino.Loot.Service
{
    public class DroppingLootService : IWorldScope
    {
        [Inject] private World _world;
        [Inject] private UnitService _unitService;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<DroppingLootConfig> _droppingLoots;
        
        public void OnWorldSetup()
        {
            _unitService.OnEnemyUnitDeath += TrySpawnLoot;
        }

        private void TrySpawnLoot(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            var lootConfig = _droppingLoots.Values.FirstOrDefault(it => it.EnemyId == unit.Model.Id);
            if (lootConfig == null)
            {
                this.Logger().Warn($"There is no loot config for enemy with id {unit.Model.Id}.");
                return;
            }
            
            var dropChance = lootConfig.DropChance;

            if (Random.value > dropChance)
            {
                return;
            }
            
            var lootId = lootConfig.Id;
            var loot = _worldObjectFactory.CreateObject(lootId, _world.Spawn.transform).GetComponent<DroppingLoot>();
            loot.transform.position = unit.GameObject.transform.position;
            loot.Init(lootConfig);
        }

        public void OnLootCollected(DroppingLootConfig collectedLoot)
        {
            switch (collectedLoot.Type)
            {
                case DroppingLootType.Exp:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnWorldCleanUp()
        {
            _unitService.OnEnemyUnitDeath -= TrySpawnLoot;
        }

    }
}
