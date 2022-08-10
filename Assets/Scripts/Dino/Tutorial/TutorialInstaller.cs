using Zenject;

namespace Dino.Tutorial
{
    public class TutorialInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<TutorialRepository>().AsSingle();
            container.BindInterfacesAndSelfTo<TutorialService>().AsSingle().NonLazy();
        }
    }
}