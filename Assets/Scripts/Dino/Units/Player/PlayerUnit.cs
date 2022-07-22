using Dino.Extension;
using Dino.Loot;
using Dino.Units.Component;
using Dino.Units.Player.Component;

namespace Dino.Units.Player
{
    public class PlayerUnit : Unit
    {
        private ActiveItemOwner _activeItemOwner;
        private PlayerAttack _playerAttack;      
        private LootCollector _lootCollector;
        
        public ActiveItemOwner ActiveItemOwner => _activeItemOwner;        
        public LootCollector LootCollector => _lootCollector;
        
        public PlayerAttack PlayerAttack => _playerAttack;

        protected override void Awake()
        {
            base.Awake();
            _activeItemOwner = gameObject.RequireComponent<ActiveItemOwner>();
            _playerAttack = gameObject.RequireComponent<PlayerAttack>();
            _lootCollector = gameObject.RequireComponentInChildren<LootCollector>();
        }
    }
}