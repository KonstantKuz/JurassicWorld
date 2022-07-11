using System.Collections.Generic;
using DinoWorldSurvival.UI.Dialog.UpgradeDialog.Model;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.UI.Dialog.UpgradeDialog.View
{
    public class UpgradeView : MonoBehaviour
    {
        private const string LEVEL_LOCALIZATION_ID = "Level";
        [SerializeField]
        private UpgradeItemView _upgradeItemPrefab;
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private TextMeshProLocalization _level;
        
        [Inject] private DiContainer _container;
        public void Init(UpgradeDialogModel dialogModel)
        {
            RemoveAllCreatedObjects();
            _level.SetTextFormatted(LEVEL_LOCALIZATION_ID, dialogModel.Level);
            CreateUpgradeItems(dialogModel.Upgrades);
        }
        private void CreateUpgradeItems(IReadOnlyCollection<UpgradeItemModel> upgrades)
        {
            upgrades.ForEach(it => {
                var itemView = _container.InstantiatePrefabForComponent<UpgradeItemView>(_upgradeItemPrefab, _root);
                itemView.Init(it);
            });
        }
        
        private void OnDisable()
        {
            RemoveAllCreatedObjects();
        }

        private void RemoveAllCreatedObjects()
        {
            _root.DestroyAllChildren();
        }
    }
}