using System.Collections;
using Dino.Location;
using Dino.Location.Service;

namespace Dino.Tutorial
{
    public abstract class WorkbenchCraftSubScenario
    {
        public readonly string Id;
        protected readonly World World;
        protected readonly WorldObjectFactory WorldObjectFactory;
        public WorkbenchCraftSubScenario(string id, World world, WorldObjectFactory worldObjectFactory)
        {
            Id = id;
            World = world;
            WorldObjectFactory = worldObjectFactory;
        }

        public abstract IEnumerator Play();
    }
}