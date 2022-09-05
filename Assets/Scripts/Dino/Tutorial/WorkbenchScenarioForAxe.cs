using System.Collections;
using System.Linq;
using DG.Tweening;

namespace Dino.Tutorial
{
    public class WorkbenchScenarioForAxe : WorkbenchScenario
    {
        private const string STONE_ID = "Stone";
        
        public override IEnumerator RunScenario()
        {
            var loots = GetTutorialLoots(STONE_ID);
            yield return WaitForLootCollected(loots);
            PlayCameraLookAtWorkbench();
            yield return WaitForCraft();
            CompleteScenario();
        }
    }
}