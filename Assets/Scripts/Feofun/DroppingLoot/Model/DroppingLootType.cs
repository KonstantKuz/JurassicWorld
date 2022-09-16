using System;

namespace Feofun.DroppingLoot.Model
{
    public enum DroppingLootType
    {
        Weapon,
        Material,
        Ammo,
    }
    
    public static class DroppingLootTypeExt
    {
        public static DroppingLootType ValueOf(string name)
        {
            return (DroppingLootType) Enum.Parse(typeof(DroppingLootType), name, true);
        }
    }
}