using UnityEngine;

namespace Feofun.Extension
{
    public static class LayerExt
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}