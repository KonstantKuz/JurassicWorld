using Feofun.App.Init;
using Zenject;

namespace Dino.ABTest.InitStep
{
    public class ABTestInitStep : AppInitStep
    {
        [Inject] 
        private ABTest _abTest;        

        protected override void Run()
        {
            _abTest.Reload();
            Next();
        }
    }
}