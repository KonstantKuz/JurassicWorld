using Dino.UI.Screen.World.Inventory;
using Dino.UI.Tutorial;
using Feofun.UI;
using Feofun.UI.Dialog;
using Feofun.UI.Loader;
using Feofun.UI.Screen;
using UnityEngine;
using Zenject;

namespace Dino.UI
{
    public class UIInstaller : MonoBehaviour
    {
        [SerializeField] 
        private UIRoot _uiRoot;
        [SerializeField]
        private Joystick _joystick;
        [SerializeField]
        private ScreenSwitcher _screenSwitcher;
        [SerializeField]
        private DialogManager _dialogManager;
        [SerializeField] 
        private TutorialUiTools _tutorialUiTools;
        [SerializeField] 
        private CraftTutorial _craftTutorial;
        public void Install(DiContainer container)
        {
            container.Bind<UIRoot>().FromInstance(_uiRoot).AsSingle();
            container.Bind<Joystick>().FromInstance(_joystick).AsSingle();
            container.Bind<ScreenSwitcher>().FromInstance(_screenSwitcher).AsSingle();
            container.Bind<DialogManager>().FromInstance(_dialogManager).AsSingle();
            container.Bind<TutorialUiTools>().FromInstance(_tutorialUiTools).AsSingle();
            container.Bind<CraftTutorial>().FromInstance(_craftTutorial).AsSingle();
            container.Bind<UiInventorySettings>().AsSingle();
            container.Bind<UILoader>().AsSingle();
        }
    }
}