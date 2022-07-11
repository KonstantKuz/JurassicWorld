using DinoWorldSurvival.UI.Screen.Main;
using DinoWorldSurvival.Units.Enemy;
using Feofun.App.Init;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using Zenject;

namespace DinoWorldSurvival.App
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