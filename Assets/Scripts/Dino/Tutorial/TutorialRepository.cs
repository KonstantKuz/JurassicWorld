using Feofun.Repository;

namespace Dino.Tutorial
{
    public class TutorialRepository: LocalPrefsSingleRepository<TutorialState>
    {
        public TutorialRepository() : base("tutorial_v1")
        {
        }
    }
}