using System.Collections;
using System.Linq;
using DG.Tweening;

namespace Dino.Tutorial
{
    public class WorkbenchScenarioForAxe : WorkbenchScenario
    {
        private const string STONES_ID = "Stones";
        
        public override IEnumerator RunScenario()
        {
            var loots = GetTutorialLoots(STONES_ID);
            yield return WaitForLootCollected(loots);
            PlayCameraLookAtWorkbench();
            yield return WaitForCraft();
            CompleteScenario();
        }
    }
}