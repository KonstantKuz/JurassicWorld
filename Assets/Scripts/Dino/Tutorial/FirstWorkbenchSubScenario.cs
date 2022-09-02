using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dino.Extension;
using Dino.Location;
using Dino.Location.Level;
using Dino.Location.Service;
using Dino.Tutorial.WaitConditions;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Dino.Tutorial
{
    public class FirstWorkbenchSubScenario : WorkbenchCraftSubScenario
    {
        private readonly IReadOnlyCollection<WorkbenchTutorialItem> _tutorialItems;
        
        public FirstWorkbenchSubScenario(string id, World world, WorldObjectFactory worldObjectFactory) : base(id, world, worldObjectFactory)
        {
            Assert.IsTrue(world.Level != null, "Tutorial can be played only on active level. ");
            
            _tutorialItems = world.Level.GetComponentsInChildren<WorkbenchTutorialItem>()
                .OrderByDescending(it => it.PointAtOrder).ToList();
        }

        public override IEnumerator Play()
        {
            var firstStepItems = _tutorialItems.Where(it => it.PointAtOrder == 1);
            firstStepItems.ForEach(it => ArrowIndicator.SpawnAbove(WorldObjectFactory, it.transform, Vector3.zero));
            yield return new WaitForAction(null);
        }
    }
}