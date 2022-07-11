using System.Collections.Generic;
using JetBrains.Annotations;
using Logger.Extension;
using Zenject;

namespace Dino.Analytics
{
    [PublicAPI]
    public class Analytics
    {
        public const char SEPARATOR = '_';

        [Inject] 
        private IEventParamProvider _eventParamProvider;
        
        
        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            this.Logger().Info("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            ReportEventToAllImpls(Events.TEST_EVENT, null);
        }
        

        private void ReportEventToAllImpls(string eventName, Dictionary<string, object> eventParams)
        {
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(eventName, eventParams, _eventParamProvider);
            }
        }
    }
}