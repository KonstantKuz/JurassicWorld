﻿namespace DinoWorldSurvival.Util
{
    public class IconPath
    {
        private const string UPGRADE_PATH_PATTERN = "Content/UI/Upgrades/{0}";    
        private const string META_UPGRADE_PATH_PATTERN = "Content/UI/Upgrades/Meta/{0}";
        private const string CURRENCY_PATH_PATTERN = "Content/UI/Currency/{0}";     
        
        public static string GetUpgrade(string upgradeId) => string.Format(UPGRADE_PATH_PATTERN, upgradeId);        
        public static string GetMetaUpgrade(string upgradeId) => string.Format(META_UPGRADE_PATH_PATTERN, upgradeId);  
        public static string GetCurrency(string currencyId) => string.Format(CURRENCY_PATH_PATTERN, currencyId);   
    }
}