using System;

namespace DinoWorldSurvival.UI.Dialog.UpgradeDialog.Model
{
    public class UpgradeItemModel
    {
        public string Id;
        public string Name;
        public LocalizableText Description;     
        public LocalizableText Level;
        public Action OnClick;
    }
}