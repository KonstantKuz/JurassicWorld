using System.Collections;
using System.Linq;
using UnityEngine;

namespace Dino.Tutorial.Scenario
{
    public class WorkbenchScenarioForBow : WorkbenchScenario
    {
        private const string FIRST_STICKS_ID = "FirstSticks";
        private const string SECOND_STICKS_ID = "SecondSticks";
        
        public override IEnumerator RunScenario()
        {
            var firstLoots = GetTutorialLoots(FIRST_STICKS_ID);
            yield return WaitForLootCollected(firstLoots);
            PlayCameraLookAtWorkbench();
            
            // yield return WaitForCraft();
            yield return new WaitForSeconds(8);
            
            var secondLoots = GetTutorialLoots(SECOND_STICKS_ID);
            PlayCameraLookAtItems(secondLoots.Select(it => it.transform).ToList());
            yield return WaitForLootCollected(secondLoots);
            PlayCameraLookAtWorkbench();
            yield return WaitForCraft();
            CompleteScenario();
        }
    }
}