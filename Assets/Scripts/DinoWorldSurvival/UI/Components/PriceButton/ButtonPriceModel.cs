using System;

namespace DinoWorldSurvival.UI.Components.PriceButton
{
    public class PriceButtonModel
    {
        public decimal Price;
        public string PriceText;
        public bool Enabled;
        public IObservable<bool> CanBuy;
        public string CurrencyIconPath;
        public bool ShowIcon => CurrencyIconPath != null;
        
    }
}