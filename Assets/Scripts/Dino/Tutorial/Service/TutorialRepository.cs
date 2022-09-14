using Dino.Tutorial.Model;
using Feofun.Repository;

namespace Dino.Tutorial.Service
{
    public class TutorialRepository: LocalPrefsSingleRepository<TutorialState>
    {
        public TutorialRepository() : base("tutorial_v1")
        {
        }
    }
}