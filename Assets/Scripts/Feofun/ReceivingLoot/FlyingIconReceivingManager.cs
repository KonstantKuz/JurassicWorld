using System.Collections.Generic;
using System.Linq;
using Feofun.ReceivingLoot.Component;
using SuperMaxim.Core.Extensions;

namespace Feofun.ReceivingLoot
{
    public class FlyingIconReceivingManager
    {
        private HashSet<FlyingIconVfxReceiver> _vfxReceivers = new HashSet<FlyingIconVfxReceiver>();

        public void ReceiveIcons(FlyingIconReceivingParams receivingParams)
        {
            _vfxReceivers.Where(receiver => receiver.VfxType == receivingParams.Type)
                         .ForEach(receiver => receiver.PlayVfx(receivingParams));
        }

        public void RegisterReceiver(FlyingIconVfxReceiver receiver)
        {
            _vfxReceivers.Add(receiver);
        }

        public void UnregisterReceiver(FlyingIconVfxReceiver receiver)
        {
            _vfxReceivers.Remove(receiver);
        }
    }
}