using System.Collections.Generic;
using Feofun.Repository;

namespace Dino.Player.Wallet
{
    public class WalletRepository : LocalPrefsSingleRepository<Dictionary<string, int>>
    {
        protected WalletRepository() : base("Wallet_v1")
        {
        }
    }
}