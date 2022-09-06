using Feofun.App.Init;
using Zenject;

namespace Dino.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private Dino.ABTest.ABTest _abTest;        

        protected override void Run()
        {
            _abTest.Reload();
            Next();
        }
    }
}