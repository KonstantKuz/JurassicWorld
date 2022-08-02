using Dino.UI.Screen.Main;
using Dino.Units.Enemy;
using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Zenject;

namespace Dino.Core.InitStep
{
    [PublicAPI]
    public class StartGameInitStep : AppInitStep
    {
        [Inject]
        private ScreenSwitcher _screenSwitcher;     
        
        protected override void Run()
        {
            DOTweenInitializer.Init();
            NavMeshInitializer.Init();
            _screenSwitcher.SwitchTo(MainScreen.URL);
            Next();
        }
    }
}