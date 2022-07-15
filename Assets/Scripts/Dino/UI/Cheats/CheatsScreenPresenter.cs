using System.Linq;
using Dino.Cheats;
using Dino.Weapon.Config;
using Dino.Weapon.Model;
using Feofun.Cheats;
using Feofun.Config;
using Feofun.UI.Components.Button;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Dino.UI.Cheats
{
    public class CheatsScreenPresenter : MonoBehaviour
    {
        [SerializeField] private ActionButton _closeButton;
        [SerializeField] private ActionButton _hideButton;

        [SerializeField] private ActionToggle _toggleConsoleButton;
        [SerializeField] private ActionToggle _toggleFPSButton;

        [SerializeField] private ActionButton _resetProgressButton;

        [SerializeField] private InputField _inputField; [SerializeField]
        private ActionButton _setLanguage;

        [SerializeField] private ActionButton _setRussianLanguage;
        [SerializeField] private ActionButton _setEnglishLanguage;

        [SerializeField] private ActionButton _testLogButton;

        [SerializeField] private DropdownWithButtonView _inventoryDropdown;
        [SerializeField] private ActionButton _removeActiveItemButton;

        [Inject] private CheatsManager _cheatsManager;
        [Inject] private CheatsActivator _cheatsActivator;
        [Inject] private ConfigCollection<WeaponId, WeaponConfig> _weaponConfigs;

        private void OnEnable()
        {
            _closeButton.Init(HideCheatsScreen);
            _hideButton.Init(DisableCheats);

            _toggleConsoleButton.Init(_cheatsManager.IsConsoleEnabled, value => { _cheatsManager.IsConsoleEnabled = value; });
            _toggleFPSButton.Init(_cheatsManager.IsFPSMonitorEnabled, value => { _cheatsManager.IsFPSMonitorEnabled = value; });

            _resetProgressButton.Init(_cheatsManager.ResetProgress);

            _setLanguage.Init(() => _cheatsManager.SetLanguage(_inputField.text));
            _setEnglishLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.English.ToString()));
            _setRussianLanguage.Init(() => _cheatsManager.SetLanguage(SystemLanguage.Russian.ToString()));
            _testLogButton.Init(() => _cheatsManager.LogTestMessage());

            _inventoryDropdown.Init(_weaponConfigs.Keys.Select(it => it.ToString()).ToList(), _cheatsManager.SetActiveItem);
            _removeActiveItemButton.Init(_cheatsManager.RemoveActiveItem);
        }

        private void DisableCheats()
        {
            _cheatsActivator.ShowOpenCheatButton(false);
            _cheatsActivator.EnableCheats(false);
            HideCheatsScreen();
        }

        private void HideCheatsScreen() => _cheatsActivator.ShowCheatsScreen(false);
    }
}