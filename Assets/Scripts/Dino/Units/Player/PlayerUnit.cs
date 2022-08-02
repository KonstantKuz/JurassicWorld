using Dino.Extension;
using Dino.Loot;
using Dino.Units.Component;
using Dino.Units.Player.Component;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerUnit : Unit
    {
        private ActiveItemOwner _activeItemOwner;
        private PlayerAttack _playerAttack;      
        private LootCollector _lootCollector;
        private NavMeshAgent _agent;
        
        public ActiveItemOwner ActiveItemOwner => _activeItemOwner;        
        public LootCollector LootCollector => _lootCollector;
        
        public PlayerAttack PlayerAttack => _playerAttack;

        private NavMeshAgent Agent => _agent ??= GetComponent<NavMeshAgent>();

        protected override void Awake()
        {
            base.Awake();
            _activeItemOwner = gameObject.RequireComponent<ActiveItemOwner>();
            _playerAttack = gameObject.RequireComponent<PlayerAttack>();
            _lootCollector = gameObject.RequireComponentInChildren<LootCollector>();
        }

        public void SetPosition(Vector3 pos)
        {
            Agent.enabled = false;
            transform.position = pos;
            Agent.enabled = true;
        }
    }
}