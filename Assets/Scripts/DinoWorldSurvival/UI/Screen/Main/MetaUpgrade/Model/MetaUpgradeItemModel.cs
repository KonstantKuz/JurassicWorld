using System;
using DinoWorldSurvival.UI.Components.PriceButton;
using Feofun.Localization;

namespace DinoWorldSurvival.UI.Screen.Main.MetaUpgrade.Model
{
    public class MetaUpgradeItemModel
    {
        public string Id;
        public LocalizableText Name;
        public LocalizableText Level;
        public bool IsMaxLevel;
        public PriceButtonModel PriceModel;
        public Action OnClick;
    }
}