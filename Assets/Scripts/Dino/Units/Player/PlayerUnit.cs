using Dino.Extension;
using Dino.Inventory.Components;
using Dino.Units.Player.Component;

namespace Dino.Units.Player
{
    public class PlayerUnit : Unit
    {
        private ActiveItemOwner _activeItemOwner;
        private PlayerAttack _playerAttack;
        
        public ActiveItemOwner ActiveItemOwner => _activeItemOwner;
        
        public PlayerAttack PlayerAttack => _playerAttack;

        protected override void Awake()
        {
            base.Awake();
            _activeItemOwner = gameObject.RequireComponent<ActiveItemOwner>();
            _playerAttack = gameObject.RequireComponent<PlayerAttack>();
        }
    }
}