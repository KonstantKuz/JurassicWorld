﻿namespace Dino.Inventory.Model
{
    public class Inventory
    {
        public UnitsMetaUpgrades UnitsUpgrades { get; }
        public Inventory()
        {
            UnitsUpgrades = new UnitsMetaUpgrades();
        }
    }
}