#if UNITY_IOS
using Feofun.App.Init;

namespace DinoWorldSurvival.IOSTransparency
{
    public class IosATTInitStep: AppInitStep
    {
        private readonly IATTListener _attListener = new IosATTListener();
        protected override void Run()
        {
            Next();
        }
        
        private void OnATTStatusReceived()
        {
            _attListener.OnStatusReceived -= OnATTStatusReceived;
            Next();
        }
    }
}
#endif