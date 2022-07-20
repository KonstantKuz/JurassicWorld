﻿namespace Dino.Util
{
    public class IconPath
    {
        private const string INVENTORY_PATH_PATTERN = "Content/UI/Inventory/{0}";
        private const string CURRENCY_PATH_PATTERN = "Content/UI/Currency/{0}";     
        
        public static string GetInventory(string itemId) => string.Format(INVENTORY_PATH_PATTERN, itemId);
        public static string GetCurrency(string currencyId) => string.Format(CURRENCY_PATH_PATTERN, currencyId);   
    }
}