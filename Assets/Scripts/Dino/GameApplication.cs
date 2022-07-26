using Dino.Analytics;
using Dino.Core.InitStep;
using Feofun.App;
using Feofun.App.Init;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Zenject;

#if UNITY_IOS
using Dino.IOSTransparency;
#endif

namespace Dino
{
    public class GameApplication : MonoBehaviour
    {
        [PublicAPI]
        public static GameApplication Instance { get; private set; }

        [Inject]
        public DiContainer Container;

        private void Awake()
        {
            Instance = this;
            AppContext.Container = Container;

#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += HandleOnPlayModeChanged;
            void HandleOnPlayModeChanged(PauseState pauseState)
            {
                
            }
#endif
            DontDestroyOnLoad(gameObject);
            RunLoadableChains();
        }
        private void RunLoadableChains()
        {
            var initSequence = gameObject.AddComponent<AppInitSequence>();

#if UNITY_IOS
            initSequence.AddStep<IosATTInitStep>();            
#endif
            initSequence.AddStep<AnalyticsInitStep>();
            initSequence.AddStep<StartGameInitStep>();
            initSequence.Next();
        }
    }
}